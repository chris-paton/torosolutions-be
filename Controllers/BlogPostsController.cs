using Microsoft.AspNetCore.Mvc;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Services;

namespace ToroSolutions.Api.Controllers
{
    /// <summary>
    /// API controller for blog post operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;
        private readonly ILogger<BlogPostsController> _logger;

        /// <summary>
        /// Initializes a new instance of the BlogPostsController.
        /// </summary>
        /// <param name="blogPostService">Blog post service.</param>
        /// <param name="logger">Logger instance.</param>
        public BlogPostsController(IBlogPostService blogPostService, ILogger<BlogPostsController> logger)
        {
            _blogPostService = blogPostService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a paginated list of published blog posts.
        /// </summary>
        /// <param name="page">Page number (default 1).</param>
        /// <param name="pageSize">Items per page (default 10).</param>
        /// <param name="category">Optional category filter.</param>
        /// <param name="featured">Optional filter for featured posts.</param>
        /// <returns>List of published blog posts.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<BlogPostDetailDto>>> GetPublishedPosts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? category = null,
            [FromQuery] bool? featured = null)
        {
            try
            {
                var posts = await _blogPostService.GetPublishedPostsAsync(page, pageSize, category, featured);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting published blog posts");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving blog posts" });
            }
        }

        /// <summary>
        /// Gets a single published blog post by slug.
        /// </summary>
        /// <param name="slug">The blog post slug.</param>
        /// <returns>Blog post details.</returns>
        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BlogPostDetailDto>> GetPostBySlug(string slug)
        {
            try
            {
                var post = await _blogPostService.GetPostBySlugAsync(slug);
                if (post == null)
                {
                    return NotFound(new { message = "Blog post not found" });
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog post by slug: {Slug}", slug);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving blog post" });
            }
        }

        /// <summary>
        /// Gets all blog posts (admin endpoint).
        /// </summary>
        /// <param name="page">Page number (default 1).</param>
        /// <param name="pageSize">Items per page (default 10).</param>
        /// <returns>All blog posts.</returns>
        [HttpGet("admin/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<BlogPostDetailDto>>> GetAllPosts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var posts = await _blogPostService.GetAllPostsAsync(page, pageSize);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all blog posts");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving blog posts" });
            }
        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        /// <param name="dto">Blog post creation data.</param>
        /// <returns>Created blog post.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BlogPostDetailDto>> CreatePost([FromBody] BlogPostDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

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

        /// <summary>
        /// Updates an existing blog post.
        /// </summary>
        /// <param name="id">The blog post ID.</param>
        /// <param name="dto">Updated blog post data.</param>
        /// <returns>Updated blog post.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BlogPostDetailDto>> UpdatePost(int id, [FromBody] BlogPostDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var post = await _blogPostService.UpdatePostAsync(id, dto);
                if (post == null)
                {
                    return NotFound(new { message = "Blog post not found" });
                }
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

        /// <summary>
        /// Deletes a blog post.
        /// </summary>
        /// <param name="id">The blog post ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var success = await _blogPostService.DeletePostAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Blog post not found" });
                }
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
