using ToroSolutions.Api.DTOs;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service interface for contact form operations.
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Submits a new contact form.
        /// </summary>
        /// <param name="dto">Contact submission data.</param>
        /// <returns>The created contact submission details.</returns>
        Task<ContactSubmissionDto> SubmitContactAsync(ContactSubmissionCreateDto dto);

        /// <summary>
        /// Gets all contact submissions (for admin use).
        /// </summary>
        /// <param name="page">Page number (starting at 1).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>List of contact submissions.</returns>
        Task<List<ContactSubmissionDto>> GetSubmissionsAsync(int page = 1, int pageSize = 20);

        /// <summary>
        /// Marks a contact submission as read.
        /// </summary>
        /// <param name="id">The submission ID.</param>
        /// <returns>The updated submission details or null if not found.</returns>
        Task<ContactSubmissionDto?> MarkAsReadAsync(int id);

        /// <summary>
        /// Gets count of unread submissions.
        /// </summary>
        /// <returns>Number of unread submissions.</returns>
        Task<int> GetUnreadCountAsync();
    }
}
