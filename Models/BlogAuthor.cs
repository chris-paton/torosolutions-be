using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.Models
{
    /// <summary>
    /// Author of a blog post. Authors are reusable across posts.
    /// </summary>
    public class BlogAuthor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string Slug { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Bio { get; set; }

        [StringLength(500)]
        public string? AvatarUrl { get; set; }

        [StringLength(100)]
        public string? Role { get; set; }

        [StringLength(500)]
        public string? TwitterUrl { get; set; }

        [StringLength(500)]
        public string? LinkedinUrl { get; set; }

        [StringLength(500)]
        public string? InstagramUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();
    }
}
