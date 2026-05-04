using Microsoft.AspNetCore.Mvc;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Services;

namespace ToroSolutions.Api.Controllers
{
    /// <summary>
    /// API controller for dashboard statistics.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;
        private readonly ICaseStudyService _caseStudyService;
        private readonly IContactService _contactService;
        private readonly ILogger<DashboardController> _logger;

        /// <summary>
        /// Initializes a new instance of the DashboardController.
        /// </summary>
        /// <param name="blogPostService">Blog post service.</param>
        /// <param name="caseStudyService">Case study service.</param>
        /// <param name="contactService">Contact service.</param>
        /// <param name="logger">Logger instance.</param>
        public DashboardController(
            IBlogPostService blogPostService,
            ICaseStudyService caseStudyService,
            IContactService contactService,
            ILogger<DashboardController> logger)
        {
            _blogPostService = blogPostService;
            _caseStudyService = caseStudyService;
            _contactService = contactService;
            _logger = logger;
        }

        /// <summary>
        /// Gets dashboard statistics.
        /// </summary>
        /// <returns>Dashboard statistics.</returns>
        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            try
            {
                // Get all posts (admin view returns drafts + published; total counts both)
                var posts = await _blogPostService.GetAllPostsAsync(page: 1, pageSize: 1000);
                var totalPosts = posts.Count;

                // Get all case studies (published)
                var caseStudies = await _caseStudyService.GetPublishedCaseStudiesAsync(page: 1, pageSize: 1000);
                var totalCaseStudies = caseStudies.Count;

                // Get all submissions
                var submissions = await _contactService.GetSubmissionsAsync(page: 1, pageSize: 1000);
                var totalSubmissions = submissions.Count;

                // Get unread count
                var unreadSubmissions = await _contactService.GetUnreadCountAsync();

                var stats = new DashboardStatsDto
                {
                    TotalPosts = totalPosts,
                    TotalCaseStudies = totalCaseStudies,
                    TotalSubmissions = totalSubmissions,
                    UnreadSubmissions = unreadSubmissions
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard statistics");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving dashboard statistics" });
            }
        }
    }
}
