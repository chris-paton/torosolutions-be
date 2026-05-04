using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.Models
{
    /// <summary>
    /// Represents a blog post in the CMS.
    /// </summary>
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Legacy category string. Prefer BlogCategoryId/BlogCategory navigation when set.
        /// Kept for backward compatibility with the existing admin UI.
        /// </summary>
        [StringLength(100)]
        public string? Category { get; set; }

        public int? BlogCategoryId { get; set; }
        public BlogCategory? BlogCategory { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Excerpt { get; set; }

        public string? FeaturedImageUrl { get; set; }

        [StringLength(200)]
        public string? FeaturedImageAlt { get; set; }

        public int? FeaturedImageWidth { get; set; }

        public int? FeaturedImageHeight { get; set; }

        /// <summary>
        /// Legacy author name string. Prefer BlogAuthorId/BlogAuthor navigation when set.
        /// </summary>
        [StringLength(100)]
        public string Author { get; set; } = "Chris Paton";

        public int? BlogAuthorId { get; set; }
        public BlogAuthor? BlogAuthor { get; set; }

        public int ReadTimeMinutes { get; set; } = 5;

        public int? WordCount { get; set; }

        [StringLength(200)]
        public string? MetaTitle { get; set; }

        [StringLength(500)]
        public string? MetaDescription { get; set; }

        [StringLength(500)]
        public string? CanonicalUrl { get; set; }

        public bool IsPublished { get; set; } = false;

        public bool IsFeatured { get; set; } = false;

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BlogPostTag> PostTags { get; set; } = new List<BlogPostTag>();
    }
}
