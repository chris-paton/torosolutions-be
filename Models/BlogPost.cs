using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.Models
{
    /// <summary>
    /// Represents a blog post in the CMS.
    /// </summary>
    public class BlogPost
    {
        /// <summary>
        /// Unique identifier for the blog post.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Title of the blog post (max 200 characters).
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
        /// Category for the blog post (max 100 characters).
        /// </summary>
        [StringLength(100)]
        public string? Category { get; set; }

        /// <summary>
        /// Full HTML content of the blog post.
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Brief excerpt of the blog post (max 500 characters).
        /// </summary>
        [StringLength(500)]
        public string? Excerpt { get; set; }

        /// <summary>
        /// URL to the featured image for the blog post.
        /// </summary>
        public string? FeaturedImageUrl { get; set; }

        /// <summary>
        /// Author of the blog post (defaults to "Chris Paton", max 100 characters).
        /// </summary>
        [StringLength(100)]
        public string Author { get; set; } = "Chris Paton";

        /// <summary>
        /// Estimated read time in minutes (defaults to 5).
        /// </summary>
        public int ReadTimeMinutes { get; set; } = 5;

        /// <summary>
        /// Indicates whether the blog post is published.
        /// </summary>
        public bool IsPublished { get; set; } = false;

        /// <summary>
        /// Indicates whether the blog post is featured on the home page.
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Timestamp when the blog post was published.
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Timestamp when the blog post was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp when the blog post was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
