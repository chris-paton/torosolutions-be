using Microsoft.AspNetCore.Mvc;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Middleware;
using ToroSolutions.Api.Services;

namespace ToroSolutions.Api.Controllers
{
    /// <summary>
    /// API controller for blog post operations.
    /// Public endpoints return the rich blog format (posts wrapped in pagination, author/category as objects, tags array).
    /// Admin endpoints (admin/all, POST, PUT, DELETE) return the legacy flat shape used by the existing admin UI.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;
        private readonly ILogger<BlogPostsController> _logger;

        public BlogPostsController(IBlogPostService blogPostService, ILogger<BlogPostsController> logger)
        {
            _blogPostService = blogPostService;
            _logger = logger;
        }

        // ===== Public (rich) =====

        /// <summary>
        /// Paginated published posts in the rich format. Supports category, tag, search and featured filters.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedBlogPostsDto>> GetPublishedPosts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12,
            [FromQuery] string? category = null,
            [FromQuery] string? tag = null,
            [FromQuery] string? search = null,
            [FromQuery] bool? featured = null)
        {
            try
            {
                var result = await _blogPostService.GetPublicPostsAsync(page, pageSize, category, tag, search, featured);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting published blog posts");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving blog posts" });
            }
        }

        /// <summary>
        /// All categories with post counts (only categories with at least one published post).
        /// </summary>
        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BlogCategoryDto>>> GetCategories()
        {
            try
            {
                return Ok(await _blogPostService.GetCategoriesAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving categories" });
            }
        }

        /// <summary>
        /// All tags with post counts (only tags used by at least one published post).
        /// </summary>
        [HttpGet("tags")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BlogTagDto>>> GetTags()
        {
            try
            {
                return Ok(await _blogPostService.GetTagsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tags");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving tags" });
            }
        }

        /// <summary>
        /// All admin posts (drafts + published). Returns the flat shape consumed by the admin UI.
        /// Defined before the {slug} route so "admin" doesn't get matched as a slug.
        /// </summary>
        [HttpGet("admin/all")]
        [ApiKeyAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BlogPostDetailDto>>> GetAllPosts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            try
            {
                return Ok(await _blogPostService.GetAllPostsAsync(page, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all blog posts");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving blog posts" });
            }
        }

        /// <summary>
        /// Posts related to a given slug (same category, falls back to most-recent published).
        /// </summary>
        [HttpGet("{slug}/related")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BlogPostSummaryDto>>> GetRelatedPosts(string slug, [FromQuery] int limit = 3)
        {
            try
            {
                return Ok(await _blogPostService.GetRelatedPostsAsync(slug, limit));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting related posts for slug: {Slug}", slug);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving related posts" });
            }
        }

        /// <summary>
        /// Single published post by slug (rich shape).
        /// </summary>
        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BlogPostPublicDto>> GetPostBySlug(string slug)
        {
            try
            {
                var post = await _blogPostService.GetPublicPostBySlugAsync(slug);
                if (post == null) return NotFound(new { message = "Blog post not found" });
                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog post by slug: {Slug}", slug);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving blog post" });
            }
        }

        // ===== Admin write operations (flat) =====

        [HttpPost]
        [ApiKeyAuthorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BlogPostDetailDto>> CreatePost([FromBody] BlogPostDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var post = await _blogPostService.CreatePostAsync(dto);
                return CreatedAtAction(nameof(GetPostBySlug), new { slug = post.Slug }, post);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid blog post data");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blog post");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error creating blog post" });
            }
        }

        [HttpPut("{id}")]
        [ApiKeyAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BlogPostDetailDto>> UpdatePost(int id, [FromBody] BlogPostDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var post = await _blogPostService.UpdatePostAsync(id, dto);
                if (post == null) return NotFound(new { message = "Blog post not found" });
                return Ok(post);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid blog post data for update");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blog post: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating blog post" });
            }
        }

        [HttpDelete("{id}")]
        [ApiKeyAuthorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var success = await _blogPostService.DeletePostAsync(id);
                if (!success) return NotFound(new { message = "Blog post not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blog post: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error deleting blog post" });
            }
        }
    }
}
