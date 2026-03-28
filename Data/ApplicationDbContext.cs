using Microsoft.EntityFrameworkCore;
using ToroSolutions.Api.Models;

namespace ToroSolutions.Api.Data
{
    /// <summary>
    /// Entity Framework Core context for Toro Solutions database.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext.
        /// </summary>
        /// <param name="options">Database context options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Blog posts in the database.
        /// </summary>
        public DbSet<BlogPost> BlogPosts { get; set; } = null!;

        /// <summary>
        /// Case studies in the database.
        /// </summary>
        public DbSet<CaseStudy> CaseStudies { get; set; } = null!;

        /// <summary>
        /// Dynamic page content.
        /// </summary>
        public DbSet<PageContent> PageContents { get; set; } = null!;

        /// <summary>
        /// Contact form submissions.
        /// </summary>
        public DbSet<ContactSubmission> ContactSubmissions { get; set; } = null!;

        /// <summary>
        /// Site-wide settings.
        /// </summary>
        public DbSet<SiteSettings> SiteSettings { get; set; } = null!;

        /// <summary>
        /// Configures the database schema and constraints.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure BlogPost constraints
            modelBuilder.Entity<BlogPost>()
                .HasIndex(b => b.Slug)
                .IsUnique();

            // Configure CaseStudy constraints
            modelBuilder.Entity<CaseStudy>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            // Configure PageContent constraints - unique combination of PageSlug and SectionKey
            modelBuilder.Entity<PageContent>()
                .HasIndex(p => new { p.PageSlug, p.SectionKey })
                .IsUnique();

            // Configure ContactSubmission indexes
            modelBuilder.Entity<ContactSubmission>()
                .HasIndex(c => c.Email);

            modelBuilder.Entity<ContactSubmission>()
                .HasIndex(c => c.IsRead);
        }
    }
}
