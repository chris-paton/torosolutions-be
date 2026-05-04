using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ToroSolutions.Api.Middleware
{
    /// <summary>
    /// Action filter that requires a valid API key for the decorated endpoint.
    /// The key is read from the X-API-Key header (or "X-Api-Key" — both accepted)
    /// and compared to the value in configuration at <c>ApiKeys:Admin</c>.
    ///
    /// Apply at action level for write endpoints (POST/PUT/DELETE) so public reads
    /// remain open. Verqos's DotNetApi adapter sends X-Api-Key by default when
    /// AuthType="api_key".
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ApiKeyAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        private const string HeaderName = "X-API-Key";
        private const string AltHeaderName = "X-Api-Key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var expectedKey = configuration?["ApiKeys:Admin"];

            if (string.IsNullOrWhiteSpace(expectedKey))
            {
                // Misconfiguration — deny rather than allow when key isn't set.
                var logger = context.HttpContext.RequestServices.GetService<ILogger<ApiKeyAuthorizeAttribute>>();
                logger?.LogError("ApiKeys:Admin is not configured. All admin endpoints are denied.");
                context.Result = new ObjectResult(new { message = "API not configured" })
                {
                    StatusCode = StatusCodes.Status503ServiceUnavailable
                };
                return;
            }

            var headers = context.HttpContext.Request.Headers;
            string? providedKey = headers[HeaderName].FirstOrDefault()
                                  ?? headers[AltHeaderName].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(providedKey) || !FixedTimeEquals(providedKey, expectedKey))
            {
                context.Result = new ObjectResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            await next();
        }

        /// <summary>
        /// Constant-time string comparison to mitigate timing attacks.
        /// </summary>
        private static bool FixedTimeEquals(string a, string b)
        {
            if (a.Length != b.Length) return false;
            var result = 0;
            for (var i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }
            return result == 0;
        }
    }
}
