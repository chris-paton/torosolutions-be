using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.Models
{
    /// <summary>
    /// Represents dynamic page content that can be edited via the CMS.
    /// Each page is divided into sections identified by section keys.
    /// </summary>
    public class PageContent
    {
        /// <summary>
        /// Unique identifier for the page content entry.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Page identifier (e.g., "home", "about", "services", "contact").
        /// </summary>
        [Required]
        [StringLength(100)]
        public string PageSlug { get; set; } = string.Empty;

        /// <summary>
        /// Identifier for the content section within the page (e.g., "hero_title", "hero_subtitle").
        /// </summary>
        [Required]
        [StringLength(100)]
        public string SectionKey { get; set; } = string.Empty;

        /// <summary>
        /// The actual content for this section.
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Type of content: "text", "html", or "json".
        /// </summary>
        [StringLength(50)]
        public string ContentType { get; set; } = "text";

        /// <summary>
        /// Timestamp when this content was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
