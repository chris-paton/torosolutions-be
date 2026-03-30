using Microsoft.AspNetCore.Mvc;
using ToroSolutions.Api.Services;

namespace ToroSolutions.Api.Controllers
{
    /// <summary>
    /// Proxies image uploads to the external image upload API.
    /// Keeps the API key server-side so it's never exposed to the frontend.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageUploadService _imageUploadService;
        private readonly ILogger<ImageController> _logger;

        public ImageController(IImageUploadService imageUploadService, ILogger<ImageController> logger)
        {
            _imageUploadService = imageUploadService;
            _logger = logger;
        }

        /// <summary>
        /// Upload an image via the external image upload API.
        /// </summary>
        /// <param name="image">The image file (jpg, jpeg, png, webp, avif).</param>
        /// <param name="folder">Optional subfolder (e.g., "blog", "case-studies").</param>
        /// <param name="filename">Optional custom filename.</param>
        /// <returns>The public URL and metadata of the uploaded image.</returns>
        [HttpPost("upload")]
        [RequestSizeLimit(20_971_520)] // 20MB to match image upload API
        public async Task<IActionResult> Upload(IFormFile image, [FromForm] string? folder = null, [FromForm] string? filename = null)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(new { error = "No image file provided" });
            }

            // Validate file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".avif" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { error = $"Invalid file type '{extension}'. Allowed: {string.Join(", ", allowedExtensions)}" });
            }

            var result = await _imageUploadService.UploadAsync(image, folder, filename);

            if (!result.Success)
            {
                _logger.LogWarning("Image upload failed: {Error}", result.Error);
                return StatusCode(502, new { error = result.Error });
            }

            return Ok(new
            {
                success = true,
                url = result.Url,
                filename = result.Filename,
                folder = result.Folder,
                size = result.Size,
                contentType = result.ContentType
            });
        }
    }
}
