using Microsoft.EntityFrameworkCore;
using ToroSolutions.Api.Models;

namespace ToroSolutions.Api.Data
{
    /// <summary>
    /// Entity Framework Core context for Toro Solutions database.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; } = null!;
        public DbSet<BlogAuthor> BlogAuthors { get; set; } = null!;
        public DbSet<BlogCategory> BlogCategories { get; set; } = null!;
        public DbSet<BlogTag> BlogTags { get; set; } = null!;
        public DbSet<BlogPostTag> BlogPostTags { get; set; } = null!;
        public DbSet<CaseStudy> CaseStudies { get; set; } = null!;
        public DbSet<PageContent> PageContents { get; set; } = null!;
        public DbSet<ContactSubmission> ContactSubmissions { get; set; } = null!;
        public DbSet<SiteSettings> SiteSettings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // BlogPost
            modelBuilder.Entity<BlogPost>()
                .HasIndex(b => b.Slug)
                .IsUnique();

            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.BlogAuthor)
                .WithMany(a => a.Posts)
                .HasForeignKey(b => b.BlogAuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.BlogCategory)
                .WithMany(c => c.Posts)
                .HasForeignKey(b => b.BlogCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // BlogAuthor
            modelBuilder.Entity<BlogAuthor>()
                .HasIndex(a => a.Slug)
                .IsUnique();

            // BlogCategory
            modelBuilder.Entity<BlogCategory>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            // BlogTag
            modelBuilder.Entity<BlogTag>()
                .HasIndex(t => t.Slug)
                .IsUnique();

            // BlogPostTag (join)
            modelBuilder.Entity<BlogPostTag>()
                .HasKey(pt => new { pt.BlogPostId, pt.BlogTagId });

            modelBuilder.Entity<BlogPostTag>()
                .HasOne(pt => pt.BlogPost)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogPostTag>()
                .HasOne(pt => pt.BlogTag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.BlogTagId)
                .OnDelete(DeleteBehavior.Cascade);

            // CaseStudy
            modelBuilder.Entity<CaseStudy>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            // PageContent
            modelBuilder.Entity<PageContent>()
                .HasIndex(p => new { p.PageSlug, p.SectionKey })
                .IsUnique();

            // ContactSubmission
            modelBuilder.Entity<ContactSubmission>()
                .HasIndex(c => c.Email);

            modelBuilder.Entity<ContactSubmission>()
                .HasIndex(c => c.IsRead);
        }
    }
}
