namespace ToroSolutions.Api.DTOs
{
    /// <summary>
    /// DTO for dashboard statistics.
    /// </summary>
    public class DashboardStatsDto
    {
        /// <summary>
        /// Total number of published blog posts.
        /// </summary>
        public int TotalPosts { get; set; }

        /// <summary>
        /// Total number of published case studies.
        /// </summary>
        public int TotalCaseStudies { get; set; }

        /// <summary>
        /// Total number of contact submissions.
        /// </summary>
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// Number of unread contact submissions.
        /// </summary>
        public int UnreadSubmissions { get; set; }
    }
}
