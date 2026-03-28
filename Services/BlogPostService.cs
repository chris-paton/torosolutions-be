using Microsoft.EntityFrameworkCore;
using ToroSolutions.Api.Data;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Models;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service implementation for blog post operations.
    /// </summary>
    public class BlogPostService : IBlogPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BlogPostService> _logger;

        /// <summary>
        /// Initializes a new instance of the BlogPostService.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="logger">Logger instance.</param>
        public BlogPostService(ApplicationDbContext context, ILogger<BlogPostService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<List<BlogPostDetailDto>> GetPublishedPostsAsync(int page = 1, int pageSize = 10, string? category = null, bool? featured = null)
        {
            try
            {
                IQueryable<BlogPost> query = _context.BlogPosts
                    .Where(p => p.IsPublished);

                if (!string.IsNullOrWhiteSpace(category))
                {
                    query = query.Where(p => p.Category == category);
                }

                if (featured.HasValue && featured.Value)
                {
                    query = query.Where(p => p.IsFeatured);
                }

                query = query.OrderByDescending(p => p.PublishedAt);

                var posts = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return posts.Select(MapToDetailDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving published blog posts");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<BlogPostDetailDto?> GetPostBySlugAsync(string slug)
        {
            try
            {
                var post = await _context.BlogPosts
                    .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

                return post == null ? null : MapToDetailDto(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blog post by slug: {Slug}", slug);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<BlogPostDetailDto>> GetAllPostsAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var posts = await _context.BlogPosts
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return posts.Select(MapToDetailDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all blog posts");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<BlogPostDetailDto> CreatePostAsync(BlogPostDto dto)
        {
            try
            {
                var post = new BlogPost
                {
                    Title = dto.Title,
                    Slug = string.IsNullOrWhiteSpace(dto.Slug) ? GenerateSlug(dto.Title) : dto.Slug,
                    Category = dto.Category,
                    Content = dto.Content,
                    Excerpt = dto.Excerpt,
                    FeaturedImageUrl = dto.FeaturedImageUrl,
                    Author = dto.Author ?? "Chris Paton",
                    ReadTimeMinutes = dto.ReadTimeMinutes ?? 5,
                    IsPublished = dto.IsPublished ?? false,
                    IsFeatured = dto.IsFeatured ?? false,
                    PublishedAt = dto.IsPublished.HasValue && dto.IsPublished.Value ? dto.PublishedAt ?? DateTime.UtcNow : null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.BlogPosts.Add(post);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Blog post created with ID: {PostId}", post.Id);
                return MapToDetailDto(post);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error creating blog post");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blog post");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<BlogPostDetailDto?> UpdatePostAsync(int id, BlogPostDto dto)
        {
            try
            {
                var post = await _context.BlogPosts.FindAsync(id);
                if (post == null)
                {
                    _logger.LogWarning("Blog post not found for update: {PostId}", id);
                    return null;
                }

                post.Title = dto.Title;
                post.Slug = string.IsNullOrWhiteSpace(dto.Slug) ? post.Slug : dto.Slug;
                post.Category = dto.Category ?? post.Category;
                post.Content = dto.Content;
                post.Excerpt = dto.Excerpt ?? post.Excerpt;
                post.FeaturedImageUrl = dto.FeaturedImageUrl ?? post.FeaturedImageUrl;
                post.Author = dto.Author ?? post.Author;
                post.ReadTimeMinutes = dto.ReadTimeMinutes ?? post.ReadTimeMinutes;
                post.IsPublished = dto.IsPublished ?? post.IsPublished;
                post.IsFeatured = dto.IsFeatured ?? post.IsFeatured;

                if (dto.IsPublished.HasValue && dto.IsPublished.Value && post.PublishedAt == null)
                {
                    post.PublishedAt = dto.PublishedAt ?? DateTime.UtcNow;
                }

                post.UpdatedAt = DateTime.UtcNow;

                _context.BlogPosts.Update(post);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Blog post updated: {PostId}", id);
                return MapToDetailDto(post);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating blog post: {PostId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blog post: {PostId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                var post = await _context.BlogPosts.FindAsync(id);
                if (post == null)
                {
                    _logger.LogWarning("Blog post not found for deletion: {PostId}", id);
                    return false;
                }

                _context.BlogPosts.Remove(post);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Blog post deleted: {PostId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blog post: {PostId}", id);
                throw;
            }
        }

        private BlogPostDetailDto MapToDetailDto(BlogPost post)
        {
            return new BlogPostDetailDto
            {
                Id = post.Id,
                Title = post.Title,
                Slug = post.Slug,
                Category = post.Category,
                Content = post.Content,
                Excerpt = post.Excerpt,
                FeaturedImageUrl = post.FeaturedImageUrl,
                Author = post.Author,
                ReadTimeMinutes = post.ReadTimeMinutes,
                IsPublished = post.IsPublished,
                IsFeatured = post.IsFeatured,
                PublishedAt = post.PublishedAt,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            };
        }

        private string GenerateSlug(string title)
        {
            return title
                .ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("--", "-")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(",", "")
                .Replace(":", "")
                .Replace("&", "and");
        }
    }
}
