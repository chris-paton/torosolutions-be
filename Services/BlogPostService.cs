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

        public BlogPostService(ApplicationDbContext context, ILogger<BlogPostService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ===== Public (rich) =====

        public async Task<PaginatedBlogPostsDto> GetPublicPostsAsync(int page = 1, int pageSize = 12, string? category = null, string? tag = null, string? search = null, bool? featured = null)
        {
            try
            {
                IQueryable<BlogPost> query = _context.BlogPosts
                    .Include(p => p.BlogAuthor)
                    .Include(p => p.BlogCategory)
                    .Include(p => p.PostTags).ThenInclude(pt => pt.BlogTag)
                    .Where(p => p.IsPublished);

                if (!string.IsNullOrWhiteSpace(category))
                {
                    var catSlug = category.ToLowerInvariant();
                    query = query.Where(p =>
                        (p.BlogCategory != null && p.BlogCategory.Slug == catSlug) ||
                        p.Category == category);
                }

                if (!string.IsNullOrWhiteSpace(tag))
                {
                    var tagSlug = tag.ToLowerInvariant();
                    query = query.Where(p => p.PostTags.Any(pt => pt.BlogTag.Slug == tagSlug));
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var term = search.ToLowerInvariant();
                    query = query.Where(p =>
                        p.Title.ToLower().Contains(term) ||
                        (p.Excerpt != null && p.Excerpt.ToLower().Contains(term)) ||
                        p.Content.ToLower().Contains(term));
                }

                if (featured.HasValue && featured.Value)
                {
                    query = query.Where(p => p.IsFeatured);
                }

                var totalCount = await query.CountAsync();
                var totalPages = pageSize > 0 ? (int)Math.Ceiling(totalCount / (double)pageSize) : 0;

                var posts = await query
                    .OrderByDescending(p => p.PublishedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginatedBlogPostsDto
                {
                    Posts = posts.Select(MapToSummary).ToList(),
                    Pagination = new PaginationDto
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalCount = totalCount,
                        TotalPages = totalPages,
                        HasNextPage = page < totalPages,
                        HasPreviousPage = page > 1,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving public blog posts");
                throw;
            }
        }

        public async Task<BlogPostPublicDto?> GetPublicPostBySlugAsync(string slug)
        {
            try
            {
                var post = await _context.BlogPosts
                    .Include(p => p.BlogAuthor)
                    .Include(p => p.BlogCategory)
                    .Include(p => p.PostTags).ThenInclude(pt => pt.BlogTag)
                    .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);

                return post == null ? null : MapToPublic(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving public blog post by slug: {Slug}", slug);
                throw;
            }
        }

        public async Task<List<BlogPostSummaryDto>> GetRelatedPostsAsync(string slug, int limit = 3)
        {
            try
            {
                var current = await _context.BlogPosts
                    .Include(p => p.BlogCategory)
                    .FirstOrDefaultAsync(p => p.Slug == slug);

                if (current == null) return new List<BlogPostSummaryDto>();

                IQueryable<BlogPost> query = _context.BlogPosts
                    .Include(p => p.BlogAuthor)
                    .Include(p => p.BlogCategory)
                    .Where(p => p.IsPublished && p.Id != current.Id);

                if (current.BlogCategoryId.HasValue)
                {
                    query = query.Where(p => p.BlogCategoryId == current.BlogCategoryId);
                }
                else if (!string.IsNullOrWhiteSpace(current.Category))
                {
                    query = query.Where(p => p.Category == current.Category);
                }

                var related = await query
                    .OrderByDescending(p => p.PublishedAt)
                    .Take(limit)
                    .ToListAsync();

                if (related.Count < limit)
                {
                    var have = related.Select(r => r.Id).ToList();
                    have.Add(current.Id);
                    var topUp = await _context.BlogPosts
                        .Include(p => p.BlogAuthor)
                        .Include(p => p.BlogCategory)
                        .Where(p => p.IsPublished && !have.Contains(p.Id))
                        .OrderByDescending(p => p.PublishedAt)
                        .Take(limit - related.Count)
                        .ToListAsync();
                    related.AddRange(topUp);
                }

                return related.Select(MapToSummary).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving related posts for: {Slug}", slug);
                throw;
            }
        }

        public async Task<List<BlogCategoryDto>> GetCategoriesAsync()
        {
            try
            {
                return await _context.BlogCategories
                    .Select(c => new BlogCategoryDto
                    {
                        Id = c.Id.ToString(),
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        PostCount = c.Posts.Count(p => p.IsPublished),
                    })
                    .Where(c => c.PostCount > 0)
                    .OrderByDescending(c => c.PostCount)
                    .ThenBy(c => c.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                throw;
            }
        }

        public async Task<List<BlogTagDto>> GetTagsAsync()
        {
            try
            {
                return await _context.BlogTags
                    .Select(t => new BlogTagDto
                    {
                        Id = t.Id.ToString(),
                        Name = t.Name,
                        Slug = t.Slug,
                        PostCount = t.PostTags.Count(pt => pt.BlogPost.IsPublished),
                    })
                    .Where(t => t.PostCount > 0)
                    .OrderByDescending(t => t.PostCount)
                    .ThenBy(t => t.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tags");
                throw;
            }
        }

        // ===== Admin (flat) =====

        public async Task<List<BlogPostDetailDto>> GetAllPostsAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var posts = await _context.BlogPosts
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return posts.Select(MapToDetail).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all blog posts");
                throw;
            }
        }

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
                    FeaturedImageAlt = dto.FeaturedImageAlt,
                    FeaturedImageWidth = dto.FeaturedImageWidth,
                    FeaturedImageHeight = dto.FeaturedImageHeight,
                    Author = dto.Author ?? "Chris Paton",
                    ReadTimeMinutes = dto.ReadTimeMinutes ?? 5,
                    WordCount = dto.WordCount,
                    MetaTitle = dto.MetaTitle,
                    MetaDescription = dto.MetaDescription,
                    CanonicalUrl = dto.CanonicalUrl,
                    IsPublished = ResolvePublishedFlag(dto.Status, dto.IsPublished, currentValue: false),
                    IsFeatured = dto.IsFeatured ?? false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                post.PublishedAt = post.IsPublished ? dto.PublishedAt ?? DateTime.UtcNow : null;

                if (!string.IsNullOrWhiteSpace(post.Author))
                {
                    var author = await GetOrCreateAuthorAsync(post.Author);
                    post.BlogAuthorId = author.Id;
                }

                if (!string.IsNullOrWhiteSpace(dto.Category))
                {
                    var category = await GetOrCreateCategoryAsync(dto.Category);
                    post.BlogCategoryId = category.Id;
                }

                _context.BlogPosts.Add(post);
                await _context.SaveChangesAsync();

                if (dto.Tags != null && dto.Tags.Count > 0)
                {
                    await SetPostTagsAsync(post.Id, dto.Tags);
                }

                _logger.LogInformation("Blog post created with ID: {PostId}", post.Id);
                return MapToDetail(post);
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
                post.FeaturedImageAlt = dto.FeaturedImageAlt ?? post.FeaturedImageAlt;
                post.FeaturedImageWidth = dto.FeaturedImageWidth ?? post.FeaturedImageWidth;
                post.FeaturedImageHeight = dto.FeaturedImageHeight ?? post.FeaturedImageHeight;
                post.Author = dto.Author ?? post.Author;
                post.ReadTimeMinutes = dto.ReadTimeMinutes ?? post.ReadTimeMinutes;
                post.WordCount = dto.WordCount ?? post.WordCount;
                post.MetaTitle = dto.MetaTitle ?? post.MetaTitle;
                post.MetaDescription = dto.MetaDescription ?? post.MetaDescription;
                post.CanonicalUrl = dto.CanonicalUrl ?? post.CanonicalUrl;
                post.IsPublished = ResolvePublishedFlag(dto.Status, dto.IsPublished, currentValue: post.IsPublished);
                post.IsFeatured = dto.IsFeatured ?? post.IsFeatured;

                if (post.IsPublished && post.PublishedAt == null)
                {
                    post.PublishedAt = dto.PublishedAt ?? DateTime.UtcNow;
                }

                if (dto.Author != null)
                {
                    var author = await GetOrCreateAuthorAsync(dto.Author);
                    post.BlogAuthorId = author.Id;
                }

                if (dto.Category != null)
                {
                    var category = await GetOrCreateCategoryAsync(dto.Category);
                    post.BlogCategoryId = category.Id;
                }

                post.UpdatedAt = DateTime.UtcNow;

                _context.BlogPosts.Update(post);
                await _context.SaveChangesAsync();

                if (dto.Tags != null)
                {
                    await SetPostTagsAsync(post.Id, dto.Tags);
                }

                _logger.LogInformation("Blog post updated: {PostId}", id);
                return MapToDetail(post);
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

        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                var post = await _context.BlogPosts
                    .Include(p => p.PostTags)
                    .FirstOrDefaultAsync(p => p.Id == id);
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

        // ===== Private helpers =====

        private async Task<BlogAuthor> GetOrCreateAuthorAsync(string name)
        {
            var slug = GenerateSlug(name);
            var existing = await _context.BlogAuthors.FirstOrDefaultAsync(a => a.Slug == slug);
            if (existing != null) return existing;

            var author = new BlogAuthor
            {
                Name = name,
                Slug = slug,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            _context.BlogAuthors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        private async Task<BlogCategory> GetOrCreateCategoryAsync(string name)
        {
            var slug = GenerateSlug(name);
            var existing = await _context.BlogCategories.FirstOrDefaultAsync(c => c.Slug == slug);
            if (existing != null) return existing;

            var category = new BlogCategory
            {
                Name = name,
                Slug = slug,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            _context.BlogCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        private async Task<BlogTag> GetOrCreateTagAsync(string name)
        {
            var trimmed = name.Trim();
            var slug = GenerateSlug(trimmed);
            var existing = await _context.BlogTags.FirstOrDefaultAsync(t => t.Slug == slug);
            if (existing != null) return existing;

            var tag = new BlogTag
            {
                Name = trimmed,
                Slug = slug,
                CreatedAt = DateTime.UtcNow,
            };
            _context.BlogTags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        private async Task SetPostTagsAsync(int postId, List<string> tagNames)
        {
            var existing = await _context.BlogPostTags.Where(pt => pt.BlogPostId == postId).ToListAsync();
            _context.BlogPostTags.RemoveRange(existing);

            foreach (var raw in tagNames.Where(t => !string.IsNullOrWhiteSpace(t)).Distinct())
            {
                var tag = await GetOrCreateTagAsync(raw);
                _context.BlogPostTags.Add(new BlogPostTag { BlogPostId = postId, BlogTagId = tag.Id });
            }
            await _context.SaveChangesAsync();
        }

        private BlogPostDetailDto MapToDetail(BlogPost post) => new()
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
            UpdatedAt = post.UpdatedAt,
        };

        private BlogPostSummaryDto MapToSummary(BlogPost post) => new()
        {
            Id = post.Id.ToString(),
            Slug = post.Slug,
            Title = post.Title,
            Excerpt = post.Excerpt ?? string.Empty,
            FeaturedImage = BuildImage(post),
            Author = new BlogAuthorSummaryDto
            {
                Name = post.BlogAuthor?.Name ?? post.Author,
                Avatar = post.BlogAuthor?.AvatarUrl,
            },
            Category = new BlogCategorySummaryDto
            {
                Name = post.BlogCategory?.Name ?? post.Category ?? "Uncategorised",
                Slug = post.BlogCategory?.Slug ?? GenerateSlug(post.Category ?? "uncategorised"),
            },
            PublishedAt = (post.PublishedAt ?? post.CreatedAt).ToString("o"),
            ReadTime = $"{post.ReadTimeMinutes} min read",
        };

        private BlogPostPublicDto MapToPublic(BlogPost post) => new()
        {
            Id = post.Id.ToString(),
            Slug = post.Slug,
            Title = post.Title,
            Excerpt = post.Excerpt ?? string.Empty,
            Content = post.Content,
            FeaturedImage = BuildImage(post),
            Author = BuildAuthor(post),
            Category = BuildCategory(post),
            Tags = post.PostTags?.Select(pt => new BlogTagDto
            {
                Id = pt.BlogTag.Id.ToString(),
                Name = pt.BlogTag.Name,
                Slug = pt.BlogTag.Slug,
            }).ToList(),
            PublishedAt = (post.PublishedAt ?? post.CreatedAt).ToString("o"),
            UpdatedAt = post.UpdatedAt.ToString("o"),
            ReadTime = $"{post.ReadTimeMinutes} min read",
            WordCount = post.WordCount,
            MetaTitle = post.MetaTitle,
            MetaDescription = post.MetaDescription,
            CanonicalUrl = post.CanonicalUrl,
            Status = post.IsPublished ? "published" : "draft",
        };

        private BlogImageDto? BuildImage(BlogPost post)
        {
            if (string.IsNullOrWhiteSpace(post.FeaturedImageUrl)) return null;
            return new BlogImageDto
            {
                Url = post.FeaturedImageUrl,
                Alt = post.FeaturedImageAlt ?? post.Title,
                Width = post.FeaturedImageWidth,
                Height = post.FeaturedImageHeight,
            };
        }

        private BlogAuthorDto BuildAuthor(BlogPost post)
        {
            if (post.BlogAuthor != null)
            {
                var social = (post.BlogAuthor.TwitterUrl != null || post.BlogAuthor.LinkedinUrl != null || post.BlogAuthor.InstagramUrl != null)
                    ? new BlogAuthorSocialDto
                    {
                        Twitter = post.BlogAuthor.TwitterUrl,
                        Linkedin = post.BlogAuthor.LinkedinUrl,
                        Instagram = post.BlogAuthor.InstagramUrl,
                    }
                    : null;

                return new BlogAuthorDto
                {
                    Id = post.BlogAuthor.Id.ToString(),
                    Name = post.BlogAuthor.Name,
                    Slug = post.BlogAuthor.Slug,
                    Bio = post.BlogAuthor.Bio,
                    Avatar = post.BlogAuthor.AvatarUrl,
                    Role = post.BlogAuthor.Role,
                    Social = social,
                };
            }

            return new BlogAuthorDto
            {
                Id = "0",
                Name = post.Author,
                Slug = GenerateSlug(post.Author),
            };
        }

        private BlogCategoryDto BuildCategory(BlogPost post)
        {
            if (post.BlogCategory != null)
            {
                return new BlogCategoryDto
                {
                    Id = post.BlogCategory.Id.ToString(),
                    Name = post.BlogCategory.Name,
                    Slug = post.BlogCategory.Slug,
                    Description = post.BlogCategory.Description,
                };
            }

            var name = post.Category ?? "Uncategorised";
            return new BlogCategoryDto
            {
                Id = "0",
                Name = name,
                Slug = GenerateSlug(name),
            };
        }

        /// <summary>
        /// Resolves the IsPublished flag, preferring the string Status field (used by Verqos)
        /// over the bool IsPublished field. Falls back to the current value if neither is set.
        /// </summary>
        private static bool ResolvePublishedFlag(string? status, bool? isPublishedFlag, bool currentValue)
        {
            if (!string.IsNullOrWhiteSpace(status))
            {
                return status.Trim().Equals("published", StringComparison.OrdinalIgnoreCase);
            }
            return isPublishedFlag ?? currentValue;
        }

        private static string GenerateSlug(string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return string.Empty;
            return source
                .ToLowerInvariant()
                .Trim()
                .Replace("&", "and")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(",", "")
                .Replace(":", "")
                .Replace("/", "-")
                .Replace(" ", "-")
                .Replace("--", "-");
        }
    }
}
