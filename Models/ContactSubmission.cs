using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.Models
{
    /// <summary>
    /// Represents a contact form submission from the website.
    /// </summary>
    public class ContactSubmission
    {
        /// <summary>
        /// Unique identifier for the contact submission.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Full name of the person submitting the form (max 100 characters).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the person submitting the form (max 200 characters).
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Company name of the person submitting (max 100 characters).
        /// </summary>
        [StringLength(100)]
        public string? Company { get; set; }

        /// <summary>
        /// Subject of the submission (max 200 characters).
        /// </summary>
        [StringLength(200)]
        public string? Subject { get; set; }

        /// <summary>
        /// Message body from the submission.
        /// </summary>
        [Required]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the submission has been read by an admin.
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// Timestamp when the submission was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
