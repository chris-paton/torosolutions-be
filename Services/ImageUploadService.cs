using System.Text.Json;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Proxies image uploads to the external image upload API at imageupload.toro-solutions.com.
    /// Keeps the API key server-side so it's never exposed to the frontend.
    /// </summary>
    public class ImageUploadService : IImageUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ImageUploadService> _logger;

        public ImageUploadService(HttpClient httpClient, IConfiguration configuration, ILogger<ImageUploadService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ImageUploadResult> UploadAsync(IFormFile file, string? folder = null, string? filename = null)
        {
            try
            {
                var uploadUrl = _configuration["ImageUpload:UploadUrl"]
                    ?? throw new InvalidOperationException("ImageUpload:UploadUrl not configured");
                var apiKey = _configuration["ImageUpload:ApiKey"]
                    ?? throw new InvalidOperationException("ImageUpload:ApiKey not configured");

                using var content = new MultipartFormDataContent();

                // Add the image file
                using var stream = file.OpenReadStream();
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "image", filename ?? file.FileName);

                // Add optional filename
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    content.Add(new StringContent(filename), "filename");
                }

                // Add optional folder
                if (!string.IsNullOrWhiteSpace(folder))
                {
                    content.Add(new StringContent(folder), "folder");
                }

                // Set API key header
                using var request = new HttpRequestMessage(HttpMethod.Post, $"{uploadUrl}/api/upload");
                request.Headers.Add("X-Api-Key", apiKey);
                request.Content = content;

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Image upload failed with status {StatusCode}: {Body}", response.StatusCode, responseBody);
                    return new ImageUploadResult
                    {
                        Success = false,
                        Error = $"Upload failed: {response.StatusCode}"
                    };
                }

                var result = JsonSerializer.Deserialize<ExternalUploadResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || !result.Success)
                {
                    return new ImageUploadResult
                    {
                        Success = false,
                        Error = "Upload returned unsuccessful response"
                    };
                }

                _logger.LogInformation("Image uploaded successfully: {Url}", result.Url);

                return new ImageUploadResult
                {
                    Success = true,
                    Url = result.Url,
                    Filename = result.Filename,
                    Folder = result.Folder ?? string.Empty,
                    Size = result.Size,
                    ContentType = result.ContentType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                return new ImageUploadResult
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

        /// <summary>
        /// Maps the JSON response from the external image upload API.
        /// </summary>
        private class ExternalUploadResponse
        {
            public bool Success { get; set; }
            public string Url { get; set; } = string.Empty;
            public string Filename { get; set; } = string.Empty;
            public string? Folder { get; set; }
            public long Size { get; set; }
            public string ContentType { get; set; } = string.Empty;
        }
    }
}
