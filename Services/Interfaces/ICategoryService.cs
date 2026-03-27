using AuthDemo.DTOs.CategoryDtos;
using AuthDemo.Helpers;

namespace AuthDemo.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResult<CategoryResponseDto>> GetAllCategoriesAsync(
            PaginationParams pagination,
            string? search = null,
            string? sortBy = "name",
            string? sortOrder = "asc"
            );
        public Task<CategoryResponseDto> GetCategoryByIdAsync(Guid id);
        public Task<CategoryResponseDto> GetCategoryBySlugAsync(string slug);
        public Task<CategoryResponseDto> CreateCategoryAsync(CategoryRequestDto categoryRequestDto);
        public Task<CategoryResponseDto> UpdateCategoryAsync(Guid uid, CategoryRequestDto categoryRequestDto);
        public Task<bool> DeleteCategoryAsync(Guid id);

    }
}
