using Microsoft.EntityFrameworkCore;
using ToroSolutions.Api.Data;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Models;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service implementation for case study operations.
    /// </summary>
    public class CaseStudyService : ICaseStudyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CaseStudyService> _logger;

        /// <summary>
        /// Initializes a new instance of the CaseStudyService.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="logger">Logger instance.</param>
        public CaseStudyService(ApplicationDbContext context, ILogger<CaseStudyService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<List<CaseStudyDetailDto>> GetPublishedCaseStudiesAsync(int page = 1, int pageSize = 10, bool? featured = null)
        {
            try
            {
                IQueryable<CaseStudy> query = _context.CaseStudies
                    .Where(cs => cs.IsPublished);

                if (featured.HasValue && featured.Value)
                {
                    query = query.Where(cs => cs.IsFeatured);
                }

                query = query.OrderByDescending(cs => cs.CreatedAt);

                var caseStudies = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return caseStudies.Select(MapToDetailDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving published case studies");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<CaseStudyDetailDto?> GetCaseStudyBySlugAsync(string slug)
        {
            try
            {
                var caseStudy = await _context.CaseStudies
                    .FirstOrDefaultAsync(cs => cs.Slug == slug && cs.IsPublished);

                return caseStudy == null ? null : MapToDetailDto(caseStudy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving case study by slug: {Slug}", slug);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<CaseStudyDetailDto>> GetAllCaseStudiesAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var caseStudies = await _context.CaseStudies
                    .OrderByDescending(cs => cs.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return caseStudies.Select(MapToDetailDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all case studies");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<CaseStudyDetailDto> CreateCaseStudyAsync(CaseStudyDto dto)
        {
            try
            {
                var caseStudy = new CaseStudy
                {
                    Title = dto.Title,
                    Slug = string.IsNullOrWhiteSpace(dto.Slug) ? GenerateSlug(dto.Title) : dto.Slug,
                    Industry = dto.Industry,
                    Services = dto.Services,
                    ResultMetric = dto.ResultMetric,
                    ResultDescription = dto.ResultDescription,
                    Content = dto.Content,
                    ClientName = dto.ClientName,
                    FeaturedImageUrl = dto.FeaturedImageUrl,
                    IsPublished = dto.IsPublished ?? false,
                    IsFeatured = dto.IsFeatured ?? false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.CaseStudies.Add(caseStudy);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Case study created with ID: {CaseStudyId}", caseStudy.Id);
                return MapToDetailDto(caseStudy);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error creating case study");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating case study");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<CaseStudyDetailDto?> UpdateCaseStudyAsync(int id, CaseStudyDto dto)
        {
            try
            {
                var caseStudy = await _context.CaseStudies.FindAsync(id);
                if (caseStudy == null)
                {
                    _logger.LogWarning("Case study not found for update: {CaseStudyId}", id);
                    return null;
                }

                caseStudy.Title = dto.Title;
                caseStudy.Slug = string.IsNullOrWhiteSpace(dto.Slug) ? caseStudy.Slug : dto.Slug;
                caseStudy.Industry = dto.Industry ?? caseStudy.Industry;
                caseStudy.Services = dto.Services ?? caseStudy.Services;
                caseStudy.ResultMetric = dto.ResultMetric ?? caseStudy.ResultMetric;
                caseStudy.ResultDescription = dto.ResultDescription ?? caseStudy.ResultDescription;
                caseStudy.Content = dto.Content;
                caseStudy.ClientName = dto.ClientName ?? caseStudy.ClientName;
                caseStudy.FeaturedImageUrl = dto.FeaturedImageUrl ?? caseStudy.FeaturedImageUrl;
                caseStudy.IsPublished = dto.IsPublished ?? caseStudy.IsPublished;
                caseStudy.IsFeatured = dto.IsFeatured ?? caseStudy.IsFeatured;
                caseStudy.UpdatedAt = DateTime.UtcNow;

                _context.CaseStudies.Update(caseStudy);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Case study updated: {CaseStudyId}", id);
                return MapToDetailDto(caseStudy);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating case study: {CaseStudyId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating case study: {CaseStudyId}", id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteCaseStudyAsync(int id)
        {
            try
            {
                var caseStudy = await _context.CaseStudies.FindAsync(id);
                if (caseStudy == null)
                {
                    _logger.LogWarning("Case study not found for deletion: {CaseStudyId}", id);
                    return false;
                }

                _context.CaseStudies.Remove(caseStudy);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Case study deleted: {CaseStudyId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting case study: {CaseStudyId}", id);
                throw;
            }
        }

        private CaseStudyDetailDto MapToDetailDto(CaseStudy caseStudy)
        {
            return new CaseStudyDetailDto
            {
                Id = caseStudy.Id,
                Title = caseStudy.Title,
                Slug = caseStudy.Slug,
                Industry = caseStudy.Industry,
                Services = caseStudy.Services,
                ResultMetric = caseStudy.ResultMetric,
                ResultDescription = caseStudy.ResultDescription,
                Content = caseStudy.Content,
                ClientName = caseStudy.ClientName,
                FeaturedImageUrl = caseStudy.FeaturedImageUrl,
                IsPublished = caseStudy.IsPublished,
                IsFeatured = caseStudy.IsFeatured,
                CreatedAt = caseStudy.CreatedAt,
                UpdatedAt = caseStudy.UpdatedAt
            };
        }

        private string GenerateSlug(string title)
        {
            return title
                .ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("--", "-")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(",", "")
                .Replace(":", "")
                .Replace("&", "and");
        }
    }
}
