using AuthDemo.Data;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CategoryExistsAsync(string name, int? parentId, Guid? uid)
        {
            return await _context.Categories.AnyAsync(c =>
                c.Name == name &&
                c.ParentId == parentId &&
                (uid == null || c.Uid == uid)
            );
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            var newCategory = await _context.Categories.AddAsync(category);
            return await Task.FromResult(newCategory.Entity);
        }

        public async Task<bool> DeleteCategoryAsync(Guid uid)
        {
            return await _context.Categories.Where(c => c.Uid == uid)
                 .ExecuteDeleteAsync() > 0;
        }

        public async Task<(List<Category>, int TotalRecords)> GetAllCategoriesAsync(PaginationParams paginationParams, string? search = null, string? sortBy = "name", string? sortDirection = "asc")
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                 c.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                 c.Description.Contains(search, StringComparison.CurrentCultureIgnoreCase) ||
                 c.Slug.Contains(search, StringComparison.CurrentCultureIgnoreCase)
);
            }

            //sorting
            var isDesc = sortDirection?.ToLower() == "desc";
            query = sortBy?.ToLower() switch
            {
                "createdat" => isDesc ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                "slug" => isDesc ? query.OrderByDescending(c => c.Slug) : query.OrderBy(c => c.Slug),
                "name" => isDesc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                _ => isDesc ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),

            };

            var totalRecords = query.Count();
            var categories = await query
                             .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                             .Take(paginationParams.PageSize)
                             .ToListAsync();

            return (categories, totalRecords);
        }

        public async Task<List<string>> GetAllCategorySlugAsync()
        {
            return await _context.Categories.Select(c => c.Slug).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid uid)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Uid == uid);
        }

        public async Task<Category> GetCategoryByPkAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> GetCategoryBySlugAsync(string slug)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
        }

        public async Task SaveChnagesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Category> UpdateCategoryAsync(Guid uid, Category category)
        {
            var affected = await _context.Categories
                  .Where(c => c.Uid == uid)
                  .ExecuteUpdateAsync(setters => setters
                   .SetProperty(c => c.Name, category.Name)
                   .SetProperty(c => c.Description, category.Description)
                   .SetProperty(c => c.Slug, category.Slug)
                   .SetProperty(c => c.ParentId, category.ParentId)
                  );
            if (affected == null) return null;

            return await GetCategoryByIdAsync(uid);
        }

    }
}
