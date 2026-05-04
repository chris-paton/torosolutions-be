using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.Models
{
    /// <summary>
    /// Tag attached to blog posts. Many-to-many with BlogPost via BlogPostTag.
    /// </summary>
    public class BlogTag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Slug { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BlogPostTag> PostTags { get; set; } = new List<BlogPostTag>();
    }
}
