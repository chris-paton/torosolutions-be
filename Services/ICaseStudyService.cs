using ToroSolutions.Api.DTOs;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service interface for case study operations.
    /// </summary>
    public interface ICaseStudyService
    {
        /// <summary>
        /// Gets a paginated list of published case studies.
        /// </summary>
        /// <param name="page">Page number (starting at 1).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <param name="featured">Optional filter for featured case studies only.</param>
        /// <returns>List of case study details.</returns>
        Task<List<CaseStudyDetailDto>> GetPublishedCaseStudiesAsync(int page = 1, int pageSize = 10, bool? featured = null);

        /// <summary>
        /// Gets a single published case study by slug.
        /// </summary>
        /// <param name="slug">The case study slug.</param>
        /// <returns>Case study details or null if not found.</returns>
        Task<CaseStudyDetailDto?> GetCaseStudyBySlugAsync(string slug);

        /// <summary>
        /// Gets all case studies (for admin use).
        /// </summary>
        /// <param name="page">Page number (starting at 1).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>List of all case study details.</returns>
        Task<List<CaseStudyDetailDto>> GetAllCaseStudiesAsync(int page = 1, int pageSize = 10);

        /// <summary>
        /// Creates a new case study.
        /// </summary>
        /// <param name="dto">Case study creation data.</param>
        /// <returns>The created case study details.</returns>
        Task<CaseStudyDetailDto> CreateCaseStudyAsync(CaseStudyDto dto);

        /// <summary>
        /// Updates an existing case study.
        /// </summary>
        /// <param name="id">The case study ID.</param>
        /// <param name="dto">Updated case study data.</param>
        /// <returns>The updated case study details.</returns>
        Task<CaseStudyDetailDto?> UpdateCaseStudyAsync(int id, CaseStudyDto dto);

        /// <summary>
        /// Deletes a case study.
        /// </summary>
        /// <param name="id">The case study ID.</param>
        /// <returns>True if deletion was successful, false if not found.</returns>
        Task<bool> DeleteCaseStudyAsync(int id);
    }
}
