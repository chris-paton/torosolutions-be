using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.DTOs
{
    /// <summary>
    /// DTO for submitting a contact form.
    /// </summary>
    public class ContactSubmissionCreateDto
    {
        /// <summary>
        /// Full name of the person submitting.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Company name.
        /// </summary>
        [StringLength(100)]
        public string? Company { get; set; }

        /// <summary>
        /// Subject of the message.
        /// </summary>
        [StringLength(200)]
        public string? Subject { get; set; }

        /// <summary>
        /// Message content.
        /// </summary>
        [Required]
        [StringLength(5000)]
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for returning contact submission details.
    /// </summary>
    public class ContactSubmissionDto
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Company name.
        /// </summary>
        public string? Company { get; set; }

        /// <summary>
        /// Subject.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Message content.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Whether the submission has been read.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
