using Microsoft.AspNetCore.Mvc;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Services;

namespace ToroSolutions.Api.Controllers
{
    /// <summary>
    /// API controller for page content operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PageContentController : ControllerBase
    {
        private readonly IPageContentService _pageContentService;
        private readonly ILogger<PageContentController> _logger;

        /// <summary>
        /// Initializes a new instance of the PageContentController.
        /// </summary>
        /// <param name="pageContentService">Page content service.</param>
        /// <param name="logger">Logger instance.</param>
        public PageContentController(IPageContentService pageContentService, ILogger<PageContentController> logger)
        {
            _pageContentService = pageContentService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all content sections for a specific page.
        /// </summary>
        /// <param name="pageSlug">The page slug (e.g., "home", "about").</param>
        /// <returns>List of page content sections.</returns>
        [HttpGet("{pageSlug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PageContentDto>>> GetPageContent(string pageSlug)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageSlug))
                {
                    return BadRequest(new { message = "Page slug is required" });
                }

                var content = await _pageContentService.GetPageContentAsync(pageSlug);
                return Ok(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting page content for page: {PageSlug}", pageSlug);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving page content" });
            }
        }

        /// <summary>
        /// Gets a specific content section.
        /// </summary>
        /// <param name="pageSlug">The page slug.</param>
        /// <param name="sectionKey">The section key.</param>
        /// <returns>Page content details.</returns>
        [HttpGet("{pageSlug}/{sectionKey}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PageContentDto>> GetSection(string pageSlug, string sectionKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageSlug) || string.IsNullOrWhiteSpace(sectionKey))
                {
                    return BadRequest(new { message = "Page slug and section key are required" });
                }

                var content = await _pageContentService.GetSectionAsync(pageSlug, sectionKey);
                if (content == null)
                {
                    return NotFound(new { message = "Content section not found" });
                }
                return Ok(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting section: {PageSlug}/{SectionKey}", pageSlug, sectionKey);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving content section" });
            }
        }

        /// <summary>
        /// Updates a page content section.
        /// </summary>
        /// <param name="dto">Page content update data.</param>
        /// <returns>Updated page content.</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PageContentDto>> UpdateSection([FromBody] PageContentUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(dto.PageSlug) || string.IsNullOrWhiteSpace(dto.SectionKey))
                {
                    return BadRequest(new { message = "Page slug and section key are required" });
                }

                var content = await _pageContentService.UpdateSectionAsync(dto);
                return Ok(content);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid page content data");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating page content");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating page content" });
            }
        }
    }
}
