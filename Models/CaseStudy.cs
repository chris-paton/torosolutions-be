using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.Models
{
    /// <summary>
    /// Represents a case study in the CMS.
    /// </summary>
    public class CaseStudy
    {
        /// <summary>
        /// Unique identifier for the case study.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Title of the case study (max 200 characters).
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly slug derived from the title (max 200 characters, unique).
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Industry of the client (max 100 characters).
        /// </summary>
        [StringLength(100)]
        public string? Industry { get; set; }

        /// <summary>
        /// Comma-separated list of services provided (max 200 characters).
        /// Example: "AI/ML, Data Engineering"
        /// </summary>
        [StringLength(200)]
        public string? Services { get; set; }

        /// <summary>
        /// Key metric result from the engagement (max 200 characters).
        /// Example: "60% faster decisions"
        /// </summary>
        [StringLength(200)]
        public string? ResultMetric { get; set; }

        /// <summary>
        /// Description of the results achieved (max 500 characters).
        /// </summary>
        [StringLength(500)]
        public string? ResultDescription { get; set; }

        /// <summary>
        /// Full HTML content of the case study.
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Name of the client (max 100 characters).
        /// </summary>
        [StringLength(100)]
        public string? ClientName { get; set; }

        /// <summary>
        /// URL to the featured image for the case study.
        /// </summary>
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// Indicates whether the case study is published.
        /// </summary>
        public bool IsPublished { get; set; } = false;

        /// <summary>
        /// Indicates whether the case study is featured on the home page.
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Timestamp when the case study was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the case study was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
