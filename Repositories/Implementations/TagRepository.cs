using AuthDemo.Data;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Repositories.Implementations
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;
        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            var newTag = await _context.Tags.AddAsync(tag);
            return await Task.FromResult(newTag.Entity);
        }

        public async Task<Tag> GetTagByIdAsync(Guid uid)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Uid == uid);
        }

        public async Task<Tag> GetTagBySlugAsync(string slug)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Slug == slug);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Tag> UpdateTagAsync(Guid uid, Tag tag)
        {
            var affected = _context.Tags
                .Where(t => t.Uid == uid)
                .ExecuteUpdateAsync(setters => setters
                 .SetProperty(t => t.Name, tag.Name)
                );
            if (affected == null) return null;
            return await GetTagByIdAsync(uid);
        }
        public async Task<bool> DeleteTagAsync(Guid uid)
        {
            return await _context.Tags.Where(t => t.Uid == uid).ExecuteDeleteAsync() > 0;
        }

        public async Task<(List<Tag>, int totalRecords)> GetAllTagsAsync(
            PaginationParams paginationParams,
            string? search = null,
            string? sortBy = null,
            string? sortDirection = "asc"
            )
        {
            var query = _context.Tags.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t =>
                t.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                t.Slug.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                );
            }

            var isDesc = sortDirection?.ToLower() == "desc";
            query = sortBy?.ToLower() switch
            {
                "name" => isDesc ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name),
                "createdat" => isDesc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
                _ => query.OrderBy(t => t.Id),
            };

            var totalRecords = query.Count();
            var tags = await query
                             .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                             .Take(paginationParams.PageSize)
                             .ToListAsync();

            return (tags, totalRecords);

        }

        public async Task<List<string>> GetAllTagSlugAsync()
        {
            return await _context.Categories.Select(c => c.Slug).ToListAsync();
        }
    }
}
