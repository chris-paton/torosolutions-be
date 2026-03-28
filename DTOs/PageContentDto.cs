using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.DTOs
{
    /// <summary>
    /// DTO for updating page content.
    /// </summary>
    public class PageContentUpdateDto
    {
        /// <summary>
        /// Page identifier.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PageSlug { get; set; } = string.Empty;

        /// <summary>
        /// Section identifier within the page.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string SectionKey { get; set; } = string.Empty;

        /// <summary>
        /// Content for the section.
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Type of content: "text", "html", or "json".
        /// </summary>
        [StringLength(50)]
        public string? ContentType { get; set; }
    }

    /// <summary>
    /// DTO for returning page content.
    /// </summary>
    public class PageContentDto
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Page identifier.
        /// </summary>
        public string PageSlug { get; set; } = string.Empty;

        /// <summary>
        /// Section identifier.
        /// </summary>
        public string SectionKey { get; set; } = string.Empty;

        /// <summary>
        /// Content.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Content type.
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Last update timestamp.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
