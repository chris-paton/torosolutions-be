using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.DTOs
{
    /// <summary>
    /// DTO for creating or updating a case study.
    /// </summary>
    public class CaseStudyDto
    {
        /// <summary>
        /// Title of the case study.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly slug (auto-generated if not provided).
        /// </summary>
        [StringLength(200)]
        public string? Slug { get; set; }

        /// <summary>
        /// Industry of the client.
        /// </summary>
        [StringLength(100)]
        public string? Industry { get; set; }

        /// <summary>
        /// Comma-separated list of services provided.
        /// </summary>
        [StringLength(200)]
        public string? Services { get; set; }

        /// <summary>
        /// Key metric result from the engagement.
        /// </summary>
        [StringLength(200)]
        public string? ResultMetric { get; set; }

        /// <summary>
        /// Description of the results achieved.
        /// </summary>
        [StringLength(500)]
        public string? ResultDescription { get; set; }

        /// <summary>
        /// Full HTML content of the case study.
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Name of the client.
        /// </summary>
        [StringLength(100)]
        public string? ClientName { get; set; }

        /// <summary>
        /// URL to the featured image.
        /// </summary>
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// Whether the case study is published.
        /// </summary>
        public bool? IsPublished { get; set; }

        /// <summary>
        /// Whether the case study is featured.
        /// </summary>
        public bool? IsFeatured { get; set; }
    }

    /// <summary>
    /// DTO for returning case study details to clients.
    /// </summary>
    public class CaseStudyDetailDto
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the case study.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly slug.
        /// </summary>
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Industry of the client.
        /// </summary>
        public string? Industry { get; set; }

        /// <summary>
        /// Services provided.
        /// </summary>
        public string? Services { get; set; }

        /// <summary>
        /// Key metric result.
        /// </summary>
        public string? ResultMetric { get; set; }

        /// <summary>
        /// Description of results.
        /// </summary>
        public string? ResultDescription { get; set; }

        /// <summary>
        /// Full HTML content.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Client name.
        /// </summary>
        public string? ClientName { get; set; }

        /// <summary>
        /// URL to the featured image.
        /// </summary>
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// Whether published.
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Whether featured.
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update timestamp.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
