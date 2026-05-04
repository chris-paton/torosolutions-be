namespace ToroSolutions.Api.DTOs
{
    /// <summary>
    /// Author information returned in public blog responses.
    /// </summary>
    public class BlogAuthorDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Avatar { get; set; }
        public string? Role { get; set; }
        public BlogAuthorSocialDto? Social { get; set; }
    }

    /// <summary>
    /// Author social links.
    /// </summary>
    public class BlogAuthorSocialDto
    {
        public string? Twitter { get; set; }
        public string? Linkedin { get; set; }
        public string? Instagram { get; set; }
    }

    /// <summary>
    /// Category information returned in public blog responses.
    /// </summary>
    public class BlogCategoryDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? PostCount { get; set; }
    }

    /// <summary>
    /// Tag information returned in public blog responses.
    /// </summary>
    public class BlogTagDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int? PostCount { get; set; }
    }

    /// <summary>
    /// Featured image with metadata.
    /// </summary>
    public class BlogImageDto
    {
        public string Url { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string? Caption { get; set; }
    }

    /// <summary>
    /// Author summary used inside post summary cards (lightweight).
    /// </summary>
    public class BlogAuthorSummaryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Avatar { get; set; }
    }

    /// <summary>
    /// Category summary used inside post summary cards (lightweight).
    /// </summary>
    public class BlogCategorySummaryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }

    /// <summary>
    /// Summary representation of a blog post (used in lists).
    /// </summary>
    public class BlogPostSummaryDto
    {
        public string Id { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public BlogImageDto? FeaturedImage { get; set; }
        public BlogAuthorSummaryDto Author { get; set; } = new();
        public BlogCategorySummaryDto Category { get; set; } = new();
        public string PublishedAt { get; set; } = string.Empty;
        public string ReadTime { get; set; } = string.Empty;
    }

    /// <summary>
    /// Full blog post returned by public endpoints. Matches the Trusted Villas blog format.
    /// </summary>
    public class BlogPostPublicDto
    {
        public string Id { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public BlogImageDto? FeaturedImage { get; set; }
        public BlogAuthorDto Author { get; set; } = new();
        public BlogCategoryDto Category { get; set; } = new();
        public List<BlogTagDto>? Tags { get; set; }
        public string PublishedAt { get; set; } = string.Empty;
        public string? UpdatedAt { get; set; }
        public string ReadTime { get; set; } = string.Empty;
        public int? WordCount { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? CanonicalUrl { get; set; }
        public string Status { get; set; } = "draft";
    }

    /// <summary>
    /// Pagination metadata returned alongside paginated lists.
    /// </summary>
    public class PaginationDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    /// <summary>
    /// Paginated wrapper for blog post summary lists.
    /// </summary>
    public class PaginatedBlogPostsDto
    {
        public List<BlogPostSummaryDto> Posts { get; set; } = new();
        public PaginationDto Pagination { get; set; } = new();
    }
}
