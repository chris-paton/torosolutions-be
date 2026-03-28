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
            // Ensure database is created
            context.Database.EnsureCreated();

            // Return if blog posts already seeded
            if (context.BlogPosts.Any())
            {
                return;
            }

            // Seed blog posts
            var blogPosts = new[]
            {
                new BlogPost
                {
                    Title = "The ROI of AI: Measuring Success Beyond the Hype",
                    Slug = "the-roi-of-ai-measuring-success-beyond-the-hype",
                    Category = "AI Strategy",
                    Content = "<h2>Introduction</h2><p>Artificial Intelligence has become a transformative technology for businesses across all industries. However, measuring return on investment (ROI) remains a critical challenge. This article explores how to quantify the value of AI initiatives and move beyond theoretical benefits to measurable business outcomes.</p><h2>Setting Clear Metrics</h2><p>Before implementing any AI solution, organizations must establish clear, measurable metrics that align with business objectives. These metrics should include both financial indicators (cost reduction, revenue increase) and operational indicators (efficiency improvement, time saved).</p><h2>Common AI Implementation Costs</h2><p>Understanding the total cost of ownership is essential for accurate ROI calculation. Costs typically include infrastructure, data preparation, model development, and ongoing maintenance. Factor in both direct and indirect costs, including team training and organizational change management.</p><h2>Real-World Success Stories</h2><p>Companies that have successfully implemented AI typically report significant improvements in productivity, customer satisfaction, and bottom-line profitability. When properly measured and tracked, AI investments can deliver 3-5x returns within the first year.</p><h2>Conclusion</h2><p>The key to demonstrating AI ROI is establishing clear metrics from the start, managing expectations realistically, and continuously monitoring performance against business objectives.</p>",
                    Excerpt = "Learn how to measure the true return on investment from AI initiatives and demonstrate business value beyond the hype.",
                    FeaturedImageUrl = "/images/blog/roi-of-ai.jpg",
                    Author = "Chris Paton",
                    ReadTimeMinutes = 7,
                    IsPublished = true,
                    IsFeatured = true,
                    PublishedAt = DateTime.UtcNow.AddDays(-30)
                },
                new BlogPost
                {
                    Title = "5 Signs Your Business Is Ready for Digital Transformation",
                    Slug = "5-signs-your-business-is-ready-for-digital-transformation",
                    Category = "Digital Transformation",
                    Content = "<h2>Is Your Business Ready?</h2><p>Digital transformation is not a one-size-fits-all initiative. Some organizations are better positioned for success than others. Understanding whether your business is ready can help you avoid costly mistakes and maximize your transformation investment.</p><h2>Sign 1: Your Team Is Frustrated With Existing Processes</h2><p>When employees consistently complain about inefficient workflows or manual tasks, it's often a sign that technology can help. Digital transformation thrives when there's internal buy-in and recognition of problems that technology can solve.</p><h2>Sign 2: You're Losing Market Share to More Agile Competitors</h2><p>Digital-first competitors can move faster and respond to market changes more quickly. If you're noticing this competitive pressure, digital transformation might be the answer.</p><h2>Sign 3: Your Data Is Siloed Across Multiple Systems</h2><p>When data lives in disparate systems with no unified view, it's difficult to make informed decisions. A sign of readiness is recognizing this problem and wanting to solve it.</p><h2>Sign 4: You Have Executive Sponsorship and Budget</h2><p>Successful transformations require leadership commitment and adequate resources. If your executives understand the need and allocate budget accordingly, you're in a stronger position.</p><h2>Sign 5: You're Willing to Invest in Change Management</h2><p>Technology alone doesn't create transformation. If your organization is prepared to address the human side of change, you're more likely to succeed.</p>",
                    Excerpt = "Assess whether your organization has the foundation for a successful digital transformation initiative.",
                    FeaturedImageUrl = "/images/blog/digital-transformation.jpg",
                    Author = "Chris Paton",
                    ReadTimeMinutes = 6,
                    IsPublished = true,
                    IsFeatured = true,
                    PublishedAt = DateTime.UtcNow.AddDays(-25)
                },
                new BlogPost
                {
                    Title = "Building Ethical AI Systems That Stakeholders Can Trust",
                    Slug = "building-ethical-ai-systems-that-stakeholders-can-trust",
                    Category = "AI Ethics",
                    Content = "<h2>The Importance of Ethical AI</h2><p>As artificial intelligence becomes increasingly prevalent in business decision-making, questions about ethics, bias, and transparency are becoming critical. Organizations that prioritize ethical AI build greater stakeholder trust and reduce regulatory risk.</p><h2>Understanding Bias in AI</h2><p>AI systems learn from historical data, which often contains human biases. Recognizing and mitigating these biases is crucial for fair and ethical AI systems. This requires diverse teams, careful data selection, and ongoing monitoring.</p><h2>Transparency and Explainability</h2><p>Stakeholders increasingly expect to understand how AI systems make decisions. Building transparency into your AI systems helps build trust and ensures accountability.</p><h2>Governance and Accountability</h2><p>Establish clear governance frameworks for AI development and deployment. Define roles and responsibilities, create audit trails, and establish processes for addressing AI system failures.</p><h2>Continuous Monitoring</h2><p>Ethical AI is not a one-time implementation. Continuous monitoring and evaluation ensures your systems remain fair and aligned with your values over time.</p>",
                    Excerpt = "Explore best practices for building AI systems that are ethical, transparent, and trustworthy to your stakeholders.",
                    FeaturedImageUrl = "/images/blog/ethical-ai.jpg",
                    Author = "Chris Paton",
                    ReadTimeMinutes = 8,
                    IsPublished = true,
                    IsFeatured = false,
                    PublishedAt = DateTime.UtcNow.AddDays(-20)
                },
                new BlogPost
                {
                    Title = "The Hidden Cost of Legacy Systems: When Technical Debt Becomes a Business Problem",
                    Slug = "hidden-cost-of-legacy-systems-when-technical-debt-becomes-business-problem",
                    Category = "Technical Debt",
                    Content = "<h2>What Is Technical Debt?</h2><p>Technical debt accumulates when organizations prioritize short-term delivery over long-term quality. Legacy systems often carry significant technical debt that creates cascading problems: slower feature development, higher maintenance costs, and increased security vulnerabilities.</p><h2>The True Cost of Legacy Systems</h2><p>While replacing legacy systems seems expensive, the costs of maintaining them often exceed replacement costs when you factor in: staff time spent on workarounds, lost business opportunities due to inflexibility, security patches and compliance work, and employee frustration driving turnover.</p><h2>Signs You Have a Legacy System Problem</h2><p>You're spending more than 30% of development time on maintenance and bug fixes. New features take significantly longer to develop. Finding developers who know the old technology is difficult. The system can't integrate with modern applications. Compliance and security updates are increasingly difficult.</p><h2>Modernization Strategies</h2><p>Complete rewrites are risky. A better approach is incremental modernization: building a modern system alongside the legacy one, gradually migrating data and functionality, and eventually retiring the old system.</p><h2>The Business Case</h2><p>Quantify the cost of technical debt and present a clear business case for modernization. The ROI often includes both cost reduction and revenue acceleration through faster feature delivery.</p>",
                    Excerpt = "Understand how technical debt from legacy systems impacts your business and explore modernization strategies.",
                    FeaturedImageUrl = "/images/blog/legacy-systems.jpg",
                    Author = "Chris Paton",
                    ReadTimeMinutes = 8,
                    IsPublished = true,
                    IsFeatured = false,
                    PublishedAt = DateTime.UtcNow.AddDays(-15)
                },
                new BlogPost
                {
                    Title = "How NLP Is Reshaping Customer Experience and Operations",
                    Slug = "how-nlp-is-reshaping-customer-experience-and-operations",
                    Category = "NLP & AI",
                    Content = "<h2>What Is Natural Language Processing?</h2><p>Natural Language Processing (NLP) is a branch of AI that focuses on understanding and generating human language. Recent advances in NLP have made it practical for business applications, from customer service chatbots to document analysis.</p><h2>Customer Service Applications</h2><p>NLP powers modern chatbots that can understand customer intent, answer common questions, and escalate complex issues to human agents. This improves customer satisfaction while reducing support costs.</p><h2>Sentiment Analysis</h2><p>Analyzing customer feedback, reviews, and social media mentions provides valuable insights into customer sentiment. Automated sentiment analysis allows organizations to quickly identify and address customer concerns.</p><h2>Document Processing</h2><p>NLP can automate the processing of contracts, invoices, and other documents, extracting key information and categorizing documents for further action. This dramatically reduces manual work and improves accuracy.</p><h2>Search and Recommendations</h2><p>NLP powers more intelligent search experiences and recommendation engines that better understand user intent and preferences.</p><h2>Implementation Considerations</h2><p>Successful NLP implementation requires quality data, clear use cases, and realistic expectations about system capabilities. Start with focused applications and expand from there.</p>",
                    Excerpt = "Discover how Natural Language Processing is transforming customer experience and business operations.",
                    FeaturedImageUrl = "/images/blog/nlp-customer-experience.jpg",
                    Author = "Chris Paton",
                    ReadTimeMinutes = 7,
                    IsPublished = true,
                    IsFeatured = false,
                    PublishedAt = DateTime.UtcNow.AddDays(-10)
                },
                new BlogPost
                {
                    Title = "Data Strategy Before Data Science: Why Foundation Matters",
                    Slug = "data-strategy-before-data-science-why-foundation-matters",
                    Category = "Data Strategy",
                    Content = "<h2>The Cart Before the Horse</h2><p>Many organizations rush into data science projects without establishing a proper data strategy. This often leads to failed projects, wasted investment, and frustrated teams. A strong data strategy is the foundation for successful analytics and AI initiatives.</p><h2>What Is a Data Strategy?</h2><p>A data strategy defines how data will be collected, managed, governed, and used to support business objectives. It includes decisions about data architecture, quality standards, security, privacy, and organizational structure.</p><h2>Key Components of a Strong Data Strategy</h2><p>Business alignment: How does data support strategic objectives? Data governance: Who owns what data and how is quality ensured? Data architecture: How will data be stored and integrated? Privacy and security: How will data be protected? Organizational structure: Do you have the right skills and roles?</p><h2>Common Pitfalls</h2><p>Ignoring data quality issues, collecting data without a clear use case in mind, failing to address privacy and compliance requirements, building complex solutions before proving simpler ones work.</p><h2>Getting Started</h2><p>Start with a clear understanding of your business objectives. Assess your current data assets and gaps. Develop a prioritized roadmap for improvements. Build executive sponsorship and allocate adequate resources.</p><h2>The Payoff</h2><p>A well-executed data strategy enables faster analytics projects, better data quality, improved security and compliance, and higher-quality business insights.</p>",
                    Excerpt = "Learn why a solid data strategy is essential before pursuing analytics and AI initiatives.",
                    FeaturedImageUrl = "/images/blog/data-strategy.jpg",
                    Author = "Chris Paton",
                    ReadTimeMinutes = 9,
                    IsPublished = true,
                    IsFeatured = false,
                    PublishedAt = DateTime.UtcNow.AddDays(-5)
                }
            };

            foreach (var post in blogPosts)
            {
                context.BlogPosts.Add(post);
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
    }
}
