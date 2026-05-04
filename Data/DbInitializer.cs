using Microsoft.EntityFrameworkCore;
using ToroSolutions.Api.Models;

namespace ToroSolutions.Api.Data
{
    /// <summary>
    /// Initializes the database with seed data.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Initializes the database with seed data if needed.
        /// </summary>
        /// <param name="context">The database context.</param>
        public static void Initialize(ApplicationDbContext context)
        {
            // Always ensure default author exists, and backfill BlogAuthorId/BlogCategoryId
            // on any pre-existing posts that don't have them set.
            EnsureDefaultAuthorAndBackfill(context);
            EnsureCategoriesAndBackfill(context);

            // Blog posts are populated by Verqos in production, not seeded here.
            // Case studies / page content / site settings are still seeded on a fresh install.
            if (context.CaseStudies.Any())
            {
                return;
            }

            // Seed case studies
            var caseStudies = new[]
            {
                new CaseStudy
                {
                    Title = "AI-Powered Analytics Platform for Financial Services Firm",
                    Slug = "ai-powered-analytics-platform-financial-services",
                    Industry = "Financial Services",
                    Services = "AI/ML, Data Engineering, Analytics",
                    ResultMetric = "60% faster decision-making",
                    ResultDescription = "Implemented real-time analytics dashboard powered by machine learning models for market analysis and risk assessment.",
                    Content = "<h2>Challenge</h2><p>A major financial services firm struggled with slow reporting and analysis cycles that prevented timely decision-making in fast-moving markets.</p><h2>Solution</h2><p>We built an AI-powered analytics platform that ingests market data in real-time, applies machine learning models for pattern recognition, and presents insights through an interactive dashboard.</p><h2>Results</h2><p>Decision-making speed increased by 60%, enabling the firm to respond faster to market opportunities. Risk detection improved by 45%, catching potential issues earlier. Operational costs decreased by 25% through automation.</p>",
                    ClientName = "Global Financial Corp",
                    FeaturedImageUrl = "/images/case-studies/financial-analytics.jpg",
                    IsPublished = true,
                    IsFeatured = true
                },
                new CaseStudy
                {
                    Title = "Predictive Maintenance System Reduces Downtime for Manufacturing Company",
                    Slug = "predictive-maintenance-manufacturing-downtime",
                    Industry = "Manufacturing",
                    Services = "IoT, Machine Learning, Predictive Analytics",
                    ResultMetric = "70% reduction in unplanned downtime",
                    ResultDescription = "Deployed sensors and ML models to predict equipment failures before they happen.",
                    Content = "<h2>Challenge</h2><p>A manufacturing company experienced frequent unplanned equipment failures causing production disruptions and costly downtime.</p><h2>Solution</h2><p>We implemented an IoT sensor network combined with machine learning models trained to detect early warning signs of equipment failure. This enables predictive maintenance scheduling.</p><h2>Results</h2><p>Unplanned downtime decreased by 70%, productivity increased by 35%, and maintenance costs decreased by 40% through optimized scheduling.</p>",
                    ClientName = "Industrial Manufacturing Ltd",
                    FeaturedImageUrl = "/images/case-studies/predictive-maintenance.jpg",
                    IsPublished = true,
                    IsFeatured = true
                },
                new CaseStudy
                {
                    Title = "Cloud-First Transformation Enables Global Expansion",
                    Slug = "cloud-first-transformation-global-expansion",
                    Industry = "Technology",
                    Services = "Cloud Migration, Infrastructure, DevOps",
                    ResultMetric = "40% faster deployment cycles",
                    ResultDescription = "Migrated legacy systems to cloud-native architecture enabling rapid global scaling.",
                    Content = "<h2>Challenge</h2><p>A growing tech company's on-premise infrastructure couldn't keep pace with their expansion plans. Deployment cycles were slow and scaling was difficult.</p><h2>Solution</h2><p>We architected and implemented a cloud-first transformation strategy, moving applications to AWS using containerization and microservices. This included establishing DevOps practices for continuous deployment.</p><h2>Results</h2><p>Deployment cycles improved by 40%, system reliability increased to 99.99%, and cloud infrastructure costs were optimized through better resource utilization. The company successfully expanded to three new markets.</p>",
                    ClientName = "TechVenture Inc",
                    FeaturedImageUrl = "/images/case-studies/cloud-transformation.jpg",
                    IsPublished = true,
                    IsFeatured = false
                },
                new CaseStudy
                {
                    Title = "NLP-Powered Customer Insights Engine Improves Engagement",
                    Slug = "nlp-customer-insights-engagement",
                    Industry = "Retail",
                    Services = "NLP, Sentiment Analysis, Customer Analytics",
                    ResultMetric = "28% increase in customer satisfaction",
                    ResultDescription = "Automated analysis of customer feedback across channels to identify trends and drive improvements.",
                    Content = "<h2>Challenge</h2><p>A major retailer collected extensive customer feedback but lacked the ability to analyze it at scale and identify actionable insights.</p><h2>Solution</h2><p>We built an NLP system that processes customer feedback from multiple channels (reviews, surveys, social media, support tickets), performs sentiment analysis, and identifies key themes and trends.</p><h2>Results</h2><p>Customer satisfaction scores increased by 28%, product improvement cycles accelerated based on customer feedback, and support team efficiency improved by 35%.</p>",
                    ClientName = "RetailCo Global",
                    FeaturedImageUrl = "/images/case-studies/nlp-insights.jpg",
                    IsPublished = true,
                    IsFeatured = false
                },
                new CaseStudy
                {
                    Title = "Enterprise Data Platform Unifies Siloed Information",
                    Slug = "enterprise-data-platform-unified-information",
                    Industry = "Healthcare",
                    Services = "Data Engineering, Data Warehouse, BI & Analytics",
                    ResultMetric = "50% faster insight delivery",
                    ResultDescription = "Consolidated data from 12 systems into a unified platform enabling enterprise-wide analytics.",
                    Content = "<h2>Challenge</h2><p>A healthcare organization had data spread across 12 different systems with no unified view, making enterprise analytics nearly impossible.</p><h2>Solution</h2><p>We designed and implemented a modern data warehouse and ETL infrastructure that consolidated data from all systems. Built a BI layer providing self-service analytics to business users.</p><h2>Results</h2><p>Data availability to analysts improved from hours to minutes (50% faster), data quality improved significantly, and the organization gained new insights into operations and patient outcomes.</p>",
                    ClientName = "Healthcare Services Plus",
                    FeaturedImageUrl = "/images/case-studies/data-platform.jpg",
                    IsPublished = true,
                    IsFeatured = false
                },
                new CaseStudy
                {
                    Title = "Retail Digital Strategy Drives Omnichannel Excellence",
                    Slug = "retail-digital-strategy-omnichannel",
                    Industry = "Retail",
                    Services = "Digital Strategy, E-Commerce, Customer Analytics",
                    ResultMetric = "35% increase in online revenue",
                    ResultDescription = "Implemented comprehensive digital strategy connecting online and offline customer experiences.",
                    Content = "<h2>Challenge</h2><p>A traditional retailer struggled to compete with pure-play e-commerce companies and needed to transform their customer experience.</p><h2>Solution</h2><p>We developed and implemented a comprehensive digital strategy that created a unified customer view, integrated online and offline experiences, and leveraged data analytics for personalization.</p><h2>Results</h2><p>Online revenue increased by 35%, overall customer satisfaction improved by 22%, repeat purchase rates increased by 18%, and inventory efficiency improved by 25%.</p>",
                    ClientName = "Retail Excellence Group",
                    FeaturedImageUrl = "/images/case-studies/retail-digital.jpg",
                    IsPublished = true,
                    IsFeatured = false
                }
            };

            foreach (var caseStudy in caseStudies)
            {
                context.CaseStudies.Add(caseStudy);
            }

            // Seed page content
            var pageContents = new[]
            {
                // Home page
                new PageContent { PageSlug = "home", SectionKey = "hero_title", Content = "Transform Your Business with Data and AI", ContentType = "text" },
                new PageContent { PageSlug = "home", SectionKey = "hero_subtitle", Content = "Strategic consulting and technology solutions for ambitious organizations", ContentType = "text" },
                new PageContent { PageSlug = "home", SectionKey = "hero_cta", Content = "Start Your Transformation", ContentType = "text" },

                // About page
                new PageContent { PageSlug = "about", SectionKey = "mission_title", Content = "Our Mission", ContentType = "text" },
                new PageContent { PageSlug = "about", SectionKey = "mission_content", Content = "To help organizations harness the power of data and artificial intelligence to achieve their boldest business objectives.", ContentType = "html" },
                new PageContent { PageSlug = "about", SectionKey = "values_title", Content = "Our Values", ContentType = "text" },
                new PageContent { PageSlug = "about", SectionKey = "values_content", Content = "<ul><li>Excellence in everything we do</li><li>Client success is our success</li><li>Innovation and continuous learning</li><li>Integrity and transparency</li><li>Diversity and inclusion</li></ul>", ContentType = "html" },

                // Services page
                new PageContent { PageSlug = "services", SectionKey = "intro_title", Content = "Our Services", ContentType = "text" },
                new PageContent { PageSlug = "services", SectionKey = "intro_content", Content = "We offer comprehensive consulting and technology services to help organizations succeed with data and AI.", ContentType = "text" },

                // Contact page
                new PageContent { PageSlug = "contact", SectionKey = "form_title", Content = "Get In Touch", ContentType = "text" },
                new PageContent { PageSlug = "contact", SectionKey = "form_subtitle", Content = "Tell us about your challenges and we'll help you find solutions.", ContentType = "text" }
            };

            foreach (var content in pageContents)
            {
                context.PageContents.Add(content);
            }

            // Seed site settings
            var settings = new SiteSettings
            {
                SiteName = "Toro Solutions",
                SiteDescription = "Strategic consulting and technology solutions for data and AI transformation",
                ContactEmail = "info@toro-solutions.com",
                PhoneNumber = "+1 (555) 123-4567",
                Address = "123 Innovation Drive, Tech City, TC 12345"
            };

            context.SiteSettings.Add(settings);

            context.SaveChanges();
        }

        /// <summary>
        /// Ensures a default "Chris Paton" author exists, then backfills BlogAuthorId on
        /// any post that has the legacy Author string but no FK set.
        /// </summary>
        private static void EnsureDefaultAuthorAndBackfill(ApplicationDbContext context)
        {
            var chrisAuthor = context.BlogAuthors.FirstOrDefault(a => a.Slug == "chris-paton");
            if (chrisAuthor == null)
            {
                chrisAuthor = new BlogAuthor
                {
                    Name = "Chris Paton",
                    Slug = "chris-paton",
                    Bio = "Founder of Toro Solutions. Strategist and technologist focused on AI, data, and digital transformation.",
                    Role = "Founder",
                    AvatarUrl = "/images/authors/chris-paton.jpg",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                context.BlogAuthors.Add(chrisAuthor);
                context.SaveChanges();
            }

            // Backfill BlogAuthorId for any existing posts that match the author name
            var unlinked = context.BlogPosts
                .Where(p => p.BlogAuthorId == null && p.Author == "Chris Paton")
                .ToList();
            foreach (var post in unlinked)
            {
                post.BlogAuthorId = chrisAuthor.Id;
            }
            if (unlinked.Count > 0)
            {
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates BlogCategory rows for each distinct legacy Category string used by posts,
        /// then backfills BlogCategoryId on any post that has the legacy Category but no FK set.
        /// </summary>
        private static void EnsureCategoriesAndBackfill(ApplicationDbContext context)
        {
            var distinctCategories = context.BlogPosts
                .Where(p => p.Category != null && p.BlogCategoryId == null)
                .Select(p => p.Category!)
                .Distinct()
                .ToList();

            foreach (var name in distinctCategories)
            {
                var slug = SlugifyForSeed(name);
                var existing = context.BlogCategories.FirstOrDefault(c => c.Slug == slug);
                if (existing == null)
                {
                    context.BlogCategories.Add(new BlogCategory
                    {
                        Name = name,
                        Slug = slug,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    });
                }
            }
            if (distinctCategories.Count > 0)
            {
                context.SaveChanges();
            }

            // Backfill FK on posts
            var postsToBackfill = context.BlogPosts
                .Where(p => p.Category != null && p.BlogCategoryId == null)
                .ToList();
            foreach (var post in postsToBackfill)
            {
                var slug = SlugifyForSeed(post.Category!);
                var category = context.BlogCategories.FirstOrDefault(c => c.Slug == slug);
                if (category != null)
                {
                    post.BlogCategoryId = category.Id;
                }
            }
            if (postsToBackfill.Count > 0)
            {
                context.SaveChanges();
            }
        }

        private static string SlugifyForSeed(string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return string.Empty;
            return source
                .ToLowerInvariant()
                .Trim()
                .Replace("&", "and")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(",", "")
                .Replace(":", "")
                .Replace("/", "-")
                .Replace(" ", "-")
                .Replace("--", "-");
        }
    }
}
