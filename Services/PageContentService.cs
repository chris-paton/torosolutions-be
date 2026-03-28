using Microsoft.EntityFrameworkCore;
using ToroSolutions.Api.Data;
using ToroSolutions.Api.DTOs;
using ToroSolutions.Api.Models;

namespace ToroSolutions.Api.Services
{
    /// <summary>
    /// Service implementation for page content operations.
    /// </summary>
    public class PageContentService : IPageContentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PageContentService> _logger;

        /// <summary>
        /// Initializes a new instance of the PageContentService.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="logger">Logger instance.</param>
        public PageContentService(ApplicationDbContext context, ILogger<PageContentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<List<PageContentDto>> GetPageContentAsync(string pageSlug)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageSlug))
                {
                    _logger.LogWarning("Page slug is empty");
                    return new List<PageContentDto>();
                }

                var contents = await _context.PageContents
                    .Where(pc => pc.PageSlug == pageSlug)
                    .ToListAsync();

                return contents.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving page content for page: {PageSlug}", pageSlug);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<PageContentDto?> GetSectionAsync(string pageSlug, string sectionKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageSlug) || string.IsNullOrWhiteSpace(sectionKey))
                {
                    _logger.LogWarning("Page slug or section key is empty");
                    return null;
                }

                var content = await _context.PageContents
                    .FirstOrDefaultAsync(pc => pc.PageSlug == pageSlug && pc.SectionKey == sectionKey);

                return content == null ? null : MapToDto(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving section: {PageSlug}/{SectionKey}", pageSlug, sectionKey);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<PageContentDto> UpdateSectionAsync(PageContentUpdateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.PageSlug) || string.IsNullOrWhiteSpace(dto.SectionKey))
                {
                    throw new ArgumentException("Page slug and section key are required");
                }

                var content = await _context.PageContents
                    .FirstOrDefaultAsync(pc => pc.PageSlug == dto.PageSlug && pc.SectionKey == dto.SectionKey);

                if (content == null)
                {
                    // Create new if doesn't exist
                    content = new PageContent
                    {
                        PageSlug = dto.PageSlug,
                        SectionKey = dto.SectionKey,
                        Content = dto.Content,
                        ContentType = dto.ContentType ?? "text",
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.PageContents.Add(content);
                    _logger.LogInformation("New page content created: {PageSlug}/{SectionKey}", dto.PageSlug, dto.SectionKey);
                }
                else
                {
                    // Update existing
                    content.Content = dto.Content;
                    content.ContentType = dto.ContentType ?? content.ContentType;
                    content.UpdatedAt = DateTime.UtcNow;

                    _context.PageContents.Update(content);
                    _logger.LogInformation("Page content updated: {PageSlug}/{SectionKey}", dto.PageSlug, dto.SectionKey);
                }

                await _context.SaveChangesAsync();
                return MapToDto(content);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating page content: {PageSlug}/{SectionKey}", dto.PageSlug, dto.SectionKey);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating page content: {PageSlug}/{SectionKey}", dto.PageSlug, dto.SectionKey);
                throw;
            }
        }

        private PageContentDto MapToDto(PageContent content)
        {
            return new PageContentDto
            {
                Id = content.Id,
                PageSlug = content.PageSlug,
                SectionKey = content.SectionKey,
                Content = content.Content,
                ContentType = content.ContentType,
                UpdatedAt = content.UpdatedAt
            };
        }
    }
}
