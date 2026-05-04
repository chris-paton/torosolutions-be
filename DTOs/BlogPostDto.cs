using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.DTOs
{
    /// <summary>
    /// Input DTO for creating or updating a blog post.
    /// Accepts both legacy flat fields (Author/Category as strings) and rich fields (Tags, MetaTitle, etc.)
    /// so both the existing admin UI and Verqos publishes can target the same endpoint.
    /// </summary>
    public class BlogPostDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Slug { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Excerpt { get; set; }

        public string? FeaturedImageUrl { get; set; }

        [StringLength(200)]
        public string? FeaturedImageAlt { get; set; }

        public int? FeaturedImageWidth { get; set; }

        public int? FeaturedImageHeight { get; set; }

        [StringLength(100)]
        public string? Author { get; set; }

        public int? ReadTimeMinutes { get; set; }

        public int? WordCount { get; set; }

        [StringLength(200)]
        public string? MetaTitle { get; set; }

        [StringLength(500)]
        public string? MetaDescription { get; set; }

        [StringLength(500)]
        public string? CanonicalUrl { get; set; }

        /// <summary>
        /// Tag names. Tags are looked up by name and created if they don't exist.
        /// </summary>
        public List<string>? Tags { get; set; }

        public bool? IsPublished { get; set; }

        public bool? IsFeatured { get; set; }

        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Optional string status accepted from Verqos ("published" | "draft" | "archived").
        /// If set, takes precedence over IsPublished. Lets Verqos's DotNetApi adapter
        /// (which sends a "status" string) flip the post live without needing a custom mapping.
        /// </summary>
        [StringLength(20)]
        public string? Status { get; set; }
    }

    /// <summary>
    /// Flat blog post DTO returned by admin endpoints (preserves the existing admin UI contract).
    /// </summary>
    public class BlogPostDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Excerpt { get; set; }
        public string? FeaturedImageUrl { get; set; }
        public string Author { get; set; } = string.Empty;
        public int ReadTimeMinutes { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
