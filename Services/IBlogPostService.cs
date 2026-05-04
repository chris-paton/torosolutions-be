using ToroSolutions.Api.DTOs;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service interface for blog post operations.
    /// </summary>
    public interface IBlogPostService
    {
        // ===== Public (rich) endpoints =====

        /// <summary>
        /// Returns paginated published posts with optional category/tag/search filters,
        /// in the rich shape consumed by the public blog frontend.
        /// </summary>
        Task<PaginatedBlogPostsDto> GetPublicPostsAsync(int page = 1, int pageSize = 12, string? category = null, string? tag = null, string? search = null, bool? featured = null);

        /// <summary>
        /// Returns a single published post by slug in the rich shape.
        /// </summary>
        Task<BlogPostPublicDto?> GetPublicPostBySlugAsync(string slug);

        /// <summary>
        /// Returns posts related to the given slug (same category, excluding the post itself).
        /// Falls back to most recent published posts if there are no category matches.
        /// </summary>
        Task<List<BlogPostSummaryDto>> GetRelatedPostsAsync(string slug, int limit = 3);

        /// <summary>
        /// Returns all categories used by published posts, with post counts.
        /// </summary>
        Task<List<BlogCategoryDto>> GetCategoriesAsync();

        /// <summary>
        /// Returns all tags used by published posts, with post counts.
        /// </summary>
        Task<List<BlogTagDto>> GetTagsAsync();

        // ===== Admin (flat) endpoints — preserved for existing admin UI =====

        /// <summary>
        /// Returns a flat list of all posts (drafts + published) for admin listing.
        /// </summary>
        Task<List<BlogPostDetailDto>> GetAllPostsAsync(int page = 1, int pageSize = 10);

        /// <summary>
        /// Creates a new post. Accepts both legacy flat input and rich input (Tags, MetaTitle, etc.).
        /// </summary>
        Task<BlogPostDetailDto> CreatePostAsync(BlogPostDto dto);

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        Task<BlogPostDetailDto?> UpdatePostAsync(int id, BlogPostDto dto);

        /// <summary>
        /// Deletes a post.
        /// </summary>
        Task<bool> DeletePostAsync(int id);
    }
}
