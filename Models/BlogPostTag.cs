namespace ToroSolutions.Api.Models
{
    /// <summary>
    /// Join entity for the many-to-many relationship between BlogPost and BlogTag.
    /// </summary>
    public class BlogPostTag
    {
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; } = null!;

        public int BlogTagId { get; set; }
        public BlogTag BlogTag { get; set; } = null!;
    }
}
