using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.DTOs
{
    /// <summary>
    /// DTO for creating or updating a blog post.
    /// </summary>
    public class BlogPostDto
    {
        /// <summary>
        /// Title of the blog post.
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
        /// Category for the blog post.
        /// </summary>
        [StringLength(100)]
        public string? Category { get; set; }

        /// <summary>
        /// Full HTML content of the blog post.
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Brief excerpt of the blog post.
        /// </summary>
        [StringLength(500)]
        public string? Excerpt { get; set; }

        /// <summary>
        /// URL to the featured image.
        /// </summary>
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// Author of the blog post.
        /// </summary>
        [StringLength(100)]
        public string? Author { get; set; }

        /// <summary>
        /// Estimated read time in minutes.
        /// </summary>
        public int? ReadTimeMinutes { get; set; }

        /// <summary>
        /// Whether the post is published.
        /// </summary>
        public bool? IsPublished { get; set; }

        /// <summary>
        /// Whether the post is featured.
        /// </summary>
        public bool? IsFeatured { get; set; }

        /// <summary>
        /// When the post was published.
        /// </summary>
        public DateTime? PublishedAt { get; set; }
    }

    /// <summary>
    /// DTO for returning blog post details to clients.
    /// </summary>
    public class BlogPostDetailDto
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the blog post.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// URL-friendly slug.
        /// </summary>
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Category for the blog post.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Full HTML content.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Brief excerpt.
        /// </summary>
        public string? Excerpt { get; set; }

        /// <summary>
        /// URL to the featured image.
        /// </summary>
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// Author name.
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Estimated read time in minutes.
        /// </summary>
        public int ReadTimeMinutes { get; set; }

        /// <summary>
        /// Whether the post is published.
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Whether the post is featured.
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// When the post was published.
        /// </summary>
        public DateTime? PublishedAt { get; set; }

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
