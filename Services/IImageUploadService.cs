namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service interface for proxying image uploads to the external image upload API.
    /// </summary>
    public interface IImageUploadService
    {
        /// <summary>
        /// Uploads an image to the external image upload API.
        /// </summary>
        /// <param name="file">The image file to upload.</param>
        /// <param name="folder">Optional subfolder (e.g., "blog", "case-studies").</param>
        /// <param name="filename">Optional custom filename.</param>
        /// <returns>The public URL of the uploaded image.</returns>
        Task<ImageUploadResult> UploadAsync(IFormFile file, string? folder = null, string? filename = null);
    }

    /// <summary>
    /// Result of an image upload operation.
    /// </summary>
    public class ImageUploadResult
    {
        public bool Success { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        public string Folder { get; set; } = string.Empty;
        public long Size { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
