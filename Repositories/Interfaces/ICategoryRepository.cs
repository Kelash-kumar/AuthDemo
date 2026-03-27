using AuthDemo.Helpers;
using AuthDemo.Models;

namespace AuthDemo.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<(List<Category>, int TotalRecords)> GetAllCategoriesAsync(
            PaginationParams paginationParams,
            string? search = null,
            string? sortBy = "name",
            string? sortDirection = "asc"
        );

        Task<Category> GetCategoryByIdAsync(Guid uid);
        Task<Category> GetCategoryByPkAsync(int id);
        Task<List<string>> GetAllCategorySlugAsync();
        Task<Category> GetCategoryBySlugAsync(string slug);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Guid uid, Category category);
        Task<bool> DeleteCategoryAsync(Guid uid);
        Task<bool> CategoryExistsAsync(string name, int? parentId, Guid? uid);
        Task SaveChnagesAsync();

    }
}
