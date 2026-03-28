using Microsoft.AspNetCore.Mvc;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Services;

namespace ToroSolutions.Api.Controllers
{
    /// <summary>
    /// API controller for contact form operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ILogger<ContactController> _logger;

        /// <summary>
        /// Initializes a new instance of the ContactController.
        /// </summary>
        /// <param name="contactService">Contact service.</param>
        /// <param name="logger">Logger instance.</param>
        public ContactController(IContactService contactService, ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        /// <summary>
        /// Submits a contact form.
        /// </summary>
        /// <param name="dto">Contact form submission data.</param>
        /// <returns>Created submission details.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContactSubmissionDto>> SubmitContact([FromBody] ContactSubmissionCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var submission = await _contactService.SubmitContactAsync(dto);
                return CreatedAtAction(null, submission);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid contact form data");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting contact form from {Email}", dto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error submitting contact form" });
            }
        }

        /// <summary>
        /// Gets all contact submissions (admin endpoint).
        /// </summary>
        /// <param name="page">Page number (default 1).</param>
        /// <param name="pageSize">Items per page (default 20).</param>
        /// <returns>List of contact submissions.</returns>
        [HttpGet("submissions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ContactSubmissionDto>>> GetSubmissions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var submissions = await _contactService.GetSubmissionsAsync(page, pageSize);
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting contact submissions");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving submissions" });
            }
        }

        /// <summary>
        /// Marks a contact submission as read.
        /// </summary>
        /// <param name="id">The submission ID.</param>
        /// <returns>Updated submission details.</returns>
        [HttpPut("submissions/{id}/read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContactSubmissionDto>> MarkAsRead(int id)
        {
            try
            {
                var submission = await _contactService.MarkAsReadAsync(id);
                if (submission == null)
                {
                    return NotFound(new { message = "Contact submission not found" });
                }
                return Ok(submission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking submission as read: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating submission" });
            }
        }
    }
}
