# Toro Solutions Backend

## Project Overview
.NET 8 Web API powering the Toro Solutions CMS. Serves blog posts, case studies, page content, and contact form submissions.

## Tech Stack
- **Framework**: .NET 8 Web API (minimal hosting pattern)
- **ORM**: Entity Framework Core 8.0 (Code-First, SQL Server)
- **Database**: SQL Server (Windows Authentication for dev)
- **Docs**: Swagger/OpenAPI (Swashbuckle)
- **Hosting**: Windows VPS (IIS/Kestrel)

## Build & Run Commands
```bash
dotnet build                    # Build project
dotnet run                      # Run (dev: http://localhost:5000, Swagger at root)
dotnet ef migrations add <Name> # Create EF migration
dotnet ef database update       # Apply migrations
dotnet test                     # Run tests (if test project exists)
```

## Configuration
- `appsettings.json` - Production config
- `appsettings.Development.json` - Dev overrides (debug logging)
- Connection string: `Server=localhost;Database=ToroSolutions;Trusted_Connection=true`
- CORS origins: `http://localhost:3000`, `https://toro-solutions.com`

## Project Structure
```
ToroSolutionsBE/
  Program.cs                    # App startup, DI, middleware pipeline
  ToroSolutions.Api.csproj      # .NET 8 project file
  Controllers/
    BlogPostsController.cs      # /api/blogposts
    CaseStudiesController.cs    # /api/casestudies
    ContactController.cs        # /api/contact
    DashboardController.cs      # /api/dashboard
    PageContentController.cs    # /api/pagecontent
  Models/
    BlogPost.cs                 # Blog post entity
    CaseStudy.cs                # Case study entity
    PageContent.cs              # CMS page content entity
    ContactSubmission.cs        # Contact form submission entity
    SiteSettings.cs             # Global site settings entity
  DTOs/                         # Request/response data transfer objects
  Services/                     # Business logic (interface + implementation per entity)
  Data/
    ApplicationDbContext.cs      # EF Core DbContext
    DbInitializer.cs            # Seed data (dev only)
  Middleware/
    ExceptionHandlingMiddleware.cs  # Global error handling
```

## API Routes

### Blog Posts (`/api/blogposts`)
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/blogposts` | Published posts (paginated, filterable by category/featured) |
| GET | `/api/blogposts/{slug}` | Single published post by slug |
| GET | `/api/blogposts/admin/all` | All posts including drafts (admin) |
| POST | `/api/blogposts` | Create post |
| PUT | `/api/blogposts/{id}` | Update post |
| DELETE | `/api/blogposts/{id}` | Delete post |

### Case Studies (`/api/casestudies`)
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/casestudies` | Published case studies (paginated, filterable) |
| GET | `/api/casestudies/{slug}` | Single published case study by slug |
| GET | `/api/casestudies/admin/all` | All case studies (admin) |
| POST | `/api/casestudies` | Create case study |
| PUT | `/api/casestudies/{id}` | Update case study |
| DELETE | `/api/casestudies/{id}` | Delete case study |

### Contact (`/api/contact`)
| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/contact` | Submit contact form |
| GET | `/api/contact/submissions` | All submissions (admin, paginated) |
| PUT | `/api/contact/submissions/{id}/read` | Mark submission as read |

### Page Content (`/api/pagecontent`)
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/pagecontent/{pageSlug}` | All sections for a page |
| GET | `/api/pagecontent/{pageSlug}/{sectionKey}` | Specific section |
| PUT | `/api/pagecontent` | Update/create section |

### Dashboard (`/api/dashboard`)
| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/dashboard/stats` | Aggregate stats (counts, unread) |

## Database Schema
- **BlogPost**: Id(PK), Title, Slug(unique), Category, Content, Excerpt, FeaturedImageUrl, Author, ReadTimeMinutes, IsPublished, IsFeatured, PublishedAt, CreatedAt, UpdatedAt
- **CaseStudy**: Id(PK), Title, Slug(unique), Industry, Services, ResultMetric, ResultDescription, Content, ClientName, FeaturedImageUrl, IsPublished, IsFeatured, CreatedAt, UpdatedAt
- **PageContent**: Id(PK), PageSlug, SectionKey, Content, ContentType, UpdatedAt. Unique: (PageSlug, SectionKey)
- **ContactSubmission**: Id(PK), FullName, Email(indexed), Company, Subject, Message, IsRead(indexed), CreatedAt
- **SiteSettings**: Id(PK), SiteName, SiteDescription, ContactEmail, PhoneNumber, Address, UpdatedAt

## Architecture Patterns
- Service layer with interface-based DI (IXxxService -> XxxService)
- DTOs for all request/response payloads (no entity exposure)
- Slug auto-generation from titles in services
- Global exception handling middleware -> structured JSON errors
- `DbInitializer.Initialize()` seeds dev data (6 blog posts, 6 case studies, page content)

## Known Issues (from handoff doc)
- **Program.cs line 6**: Uses `WebApplicationBuilder.CreateBuilder()` - must be `WebApplication.CreateBuilder()` (compile error)
- No EF migrations exist - uses `EnsureCreated()` only (needs migrations for production)
- No authentication/authorization on admin endpoints
- CORS fallback allows any origin when config is empty

## Frontend Repo
The Next.js frontend lives at `../ToroSolutionsFE/`. See that repo's CLAUDE.md for frontend details.
