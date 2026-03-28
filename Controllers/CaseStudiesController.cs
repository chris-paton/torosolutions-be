using Microsoft.AspNetCore.Mvc;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Services;

namespace ToroSolutions.Api.Controllers
{
    /// <summary>
    /// API controller for case study operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CaseStudiesController : ControllerBase
    {
        private readonly ICaseStudyService _caseStudyService;
        private readonly ILogger<CaseStudiesController> _logger;

        /// <summary>
        /// Initializes a new instance of the CaseStudiesController.
        /// </summary>
        /// <param name="caseStudyService">Case study service.</param>
        /// <param name="logger">Logger instance.</param>
        public CaseStudiesController(ICaseStudyService caseStudyService, ILogger<CaseStudiesController> logger)
        {
            _caseStudyService = caseStudyService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a paginated list of published case studies.
        /// </summary>
        /// <param name="page">Page number (default 1).</param>
        /// <param name="pageSize">Items per page (default 10).</param>
        /// <param name="featured">Optional filter for featured case studies.</param>
        /// <returns>List of published case studies.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CaseStudyDetailDto>>> GetPublishedCaseStudies(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? featured = null)
        {
            try
            {
                var caseStudies = await _caseStudyService.GetPublishedCaseStudiesAsync(page, pageSize, featured);
                return Ok(caseStudies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting published case studies");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving case studies" });
            }
        }

        /// <summary>
        /// Gets a single published case study by slug.
        /// </summary>
        /// <param name="slug">The case study slug.</param>
        /// <returns>Case study details.</returns>
        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CaseStudyDetailDto>> GetCaseStudyBySlug(string slug)
        {
            try
            {
                var caseStudy = await _caseStudyService.GetCaseStudyBySlugAsync(slug);
                if (caseStudy == null)
                {
                    return NotFound(new { message = "Case study not found" });
                }
                return Ok(caseStudy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting case study by slug: {Slug}", slug);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving case study" });
            }
        }

        /// <summary>
        /// Gets all case studies (admin endpoint).
        /// </summary>
        /// <param name="page">Page number (default 1).</param>
        /// <param name="pageSize">Items per page (default 10).</param>
        /// <returns>All case studies.</returns>
        [HttpGet("admin/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CaseStudyDetailDto>>> GetAllCaseStudies(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var caseStudies = await _caseStudyService.GetAllCaseStudiesAsync(page, pageSize);
                return Ok(caseStudies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all case studies");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving case studies" });
            }
        }

        /// <summary>
        /// Creates a new case study.
        /// </summary>
        /// <param name="dto">Case study creation data.</param>
        /// <returns>Created case study.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CaseStudyDetailDto>> CreateCaseStudy([FromBody] CaseStudyDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var caseStudy = await _caseStudyService.CreateCaseStudyAsync(dto);
                return CreatedAtAction(nameof(GetCaseStudyBySlug), new { slug = caseStudy.Slug }, caseStudy);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid case study data");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating case study");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error creating case study" });
            }
        }

        /// <summary>
        /// Updates an existing case study.
        /// </summary>
        /// <param name="id">The case study ID.</param>
        /// <param name="dto">Updated case study data.</param>
        /// <returns>Updated case study.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CaseStudyDetailDto>> UpdateCaseStudy(int id, [FromBody] CaseStudyDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var caseStudy = await _caseStudyService.UpdateCaseStudyAsync(id, dto);
                if (caseStudy == null)
                {
                    return NotFound(new { message = "Case study not found" });
                }
                return Ok(caseStudy);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid case study data for update");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating case study: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating case study" });
            }
        }

        /// <summary>
        /// Deletes a case study.
        /// </summary>
        /// <param name="id">The case study ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCaseStudy(int id)
        {
            try
            {
                var success = await _caseStudyService.DeleteCaseStudyAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Case study not found" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting case study: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error deleting case study" });
            }
        }
    }
}
