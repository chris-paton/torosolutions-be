using Microsoft.EntityFrameworkCore;
using ToroSolutions.Api.Data;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Models;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service implementation for contact form operations.
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContactService> _logger;

        /// <summary>
        /// Initializes a new instance of the ContactService.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="logger">Logger instance.</param>
        public ContactService(ApplicationDbContext context, ILogger<ContactService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ContactSubmissionDto> SubmitContactAsync(ContactSubmissionCreateDto dto)
        {
            try
            {
                var submission = new ContactSubmission
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Company = dto.Company,
                    Subject = dto.Subject,
                    Message = dto.Message,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ContactSubmissions.Add(submission);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Contact submission created from {Email}: {SubmissionId}", dto.Email, submission.Id);
                return MapToDto(submission);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error submitting contact form from {Email}", dto.Email);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting contact form from {Email}", dto.Email);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<ContactSubmissionDto>> GetSubmissionsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                var submissions = await _context.ContactSubmissions
                    .OrderByDescending(cs => cs.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return submissions.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ContactSubmissionDto?> MarkAsReadAsync(int id)
        {
            try
            {
                var submission = await _context.ContactSubmissions.FindAsync(id);
                if (submission == null)
                {
                    _logger.LogWarning("Contact submission not found for marking as read: {SubmissionId}", id);
                    return null;
                }

                submission.IsRead = true;
                _context.ContactSubmissions.Update(submission);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Contact submission marked as read: {SubmissionId}", id);
                return MapToDto(submission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking contact submission as read: {SubmissionId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<int> GetUnreadCountAsync()
        {
            try
            {
                return await _context.ContactSubmissions
                    .CountAsync(cs => !cs.IsRead);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread count");
                throw;
            }
        }

        private ContactSubmissionDto MapToDto(ContactSubmission submission)
        {
            return new ContactSubmissionDto
            {
                Id = submission.Id,
                FullName = submission.FullName,
                Email = submission.Email,
                Company = submission.Company,
                Subject = submission.Subject,
                Message = submission.Message,
                IsRead = submission.IsRead,
                CreatedAt = submission.CreatedAt
            };
        }
    }
}
