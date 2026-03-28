using ToroSolutions.Api.DTOs;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service interface for blog post operations.
    /// </summary>
    public interface IBlogPostService
    {
        /// <summary>
        /// Gets a paginated list of published blog posts.
        /// </summary>
        /// <param name="page">Page number (starting at 1).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <param name="category">Optional category filter.</param>
        /// <param name="featured">Optional filter for featured posts only.</param>
        /// <returns>List of blog post details.</returns>
        Task<List<BlogPostDetailDto>> GetPublishedPostsAsync(int page = 1, int pageSize = 10, string? category = null, bool? featured = null);

        /// <summary>
        /// Gets a single published blog post by slug.
        /// </summary>
        /// <param name="slug">The blog post slug.</param>
        /// <returns>Blog post details or null if not found.</returns>
        Task<BlogPostDetailDto?> GetPostBySlugAsync(string slug);

        /// <summary>
        /// Gets all blog posts (for admin use).
        /// </summary>
        /// <param name="page">Page number (starting at 1).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>List of all blog post details.</returns>
        Task<List<BlogPostDetailDto>> GetAllPostsAsync(int page = 1, int pageSize = 10);

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="dto">Blog post creation data.</param>
        /// <returns>The created blog post details.</returns>
        Task<BlogPostDetailDto> CreatePostAsync(BlogPostDto dto);

        /// <summary>
        /// Updates an existing blog post.
        /// </summary>
        /// <param name="id">The blog post ID.</param>
        /// <param name="dto">Updated blog post data.</param>
        /// <returns>The updated blog post details.</returns>
        Task<BlogPostDetailDto?> UpdatePostAsync(int id, BlogPostDto dto);

        /// <summary>
        /// Deletes a blog post.
        /// </summary>
        /// <param name="id">The blog post ID.</param>
        /// <returns>True if deletion was successful, false if not found.</returns>
        Task<bool> DeletePostAsync(int id);
    }
}
