using ToroSolutions.Api.DTOs;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service interface for page content operations.
    /// </summary>
    public interface IPageContentService
    {
        /// <summary>
        /// Gets all content sections for a specific page.
        /// </summary>
        /// <param name="pageSlug">The page slug (e.g., "home", "about").</param>
        /// <returns>List of page content sections.</returns>
        Task<List<PageContentDto>> GetPageContentAsync(string pageSlug);

        /// <summary>
        /// Gets a specific content section.
        /// </summary>
        /// <param name="pageSlug">The page slug.</param>
        /// <param name="sectionKey">The section key.</param>
        /// <returns>Page content details or null if not found.</returns>
        Task<PageContentDto?> GetSectionAsync(string pageSlug, string sectionKey);

        /// <summary>
        /// Updates a page content section.
        /// </summary>
        /// <param name="dto">Page content update data.</param>
        /// <returns>The updated page content.</returns>
        Task<PageContentDto> UpdateSectionAsync(PageContentUpdateDto dto);
    }
}
