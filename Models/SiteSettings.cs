using System.ComponentModel.DataAnnotations;

namespace ToroSolutions.Api.Models
{
    /// <summary>
    /// Represents global site settings for Toro Solutions.
    /// </summary>
    public class SiteSettings
    {
        /// <summary>
        /// Unique identifier for site settings.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Site name/title.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string SiteName { get; set; } = "Toro Solutions";

        /// <summary>
        /// Site description for SEO.
        /// </summary>
        [StringLength(500)]
        public string? SiteDescription { get; set; }

        /// <summary>
        /// Contact email for the organization.
        /// </summary>
        [EmailAddress]
        [StringLength(200)]
        public string? ContactEmail { get; set; }

        /// <summary>
        /// Phone number for the organization.
        /// </summary>
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Physical address of the organization.
        /// </summary>
        [StringLength(500)]
        public string? Address { get; set; }

        /// <summary>
        /// Timestamp when settings were last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
