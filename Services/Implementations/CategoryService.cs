using AuthDemo.DTOs.CategoryDtos;
using AuthDemo.Exceptions;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using AuthDemo.Services.Interfaces;

namespace AuthDemo.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISlugService _slugService;
        public CategoryService(ICategoryRepository categoryRepository, ISlugService slugService)
        {
            _categoryRepository = categoryRepository;
            _slugService = slugService;
        }

        public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryRequestDto categoryRequestDto)
        {
            var categoryName = categoryRequestDto.Name.Trim();

            if (categoryRequestDto.ParentId.HasValue)
            {
                var parentExist = await _categoryRepository.GetCategoryByPkAsync(categoryRequestDto.ParentId.Value);
                if (parentExist == null)
                {
                    throw new NotFoundException($"Parent category with id {categoryRequestDto.ParentId.Value} not found.");
                }
            }

            var exists = await _categoryRepository.CategoryExistsAsync(categoryName, categoryRequestDto.ParentId, null);

            if (exists) throw new ConflictException($"Category '{categoryName}' already exists.");

            //Geneate slug from name
            var existingSlugs = await _categoryRepository.GetAllCategorySlugAsync() ?? new List<string>();
            var slug = _slugService.GenerateUnique(categoryRequestDto.Name, existingSlugs);

            var category = new Category
            {
                Name = categoryRequestDto.Name.Trim(),
                Slug = slug,
                Description = categoryRequestDto.Description.Trim(),
                ParentId = categoryRequestDto.ParentId
            };

            await _categoryRepository.CreateCategoryAsync(category);
            await _categoryRepository.SaveChnagesAsync();

            return MapToCategoryResponseDto(category);

        }

        public async Task<CategoryResponseDto> GetCategoryByIdAsync(Guid id)
        {
            var result = await _categoryRepository.GetCategoryByIdAsync(id);

            if (result == null)
            {
                throw new NotFoundException($"Category With Guid '{id}' not Exist");
            }

            return MapToCategoryResponseDto(result);
        }

        public async Task<CategoryResponseDto> GetCategoryBySlugAsync(string slug)
        {
            if (slug == null) throw new ValidationException("Slug can not be Null.");

            var result = await _categoryRepository.GetCategoryBySlugAsync(slug);

            if (result == null)
            {
                throw new NotFoundException($"Category With slug '{slug}' not Exist");
            }

            return MapToCategoryResponseDto(result);
        }

        public async Task<PagedResult<CategoryResponseDto>> GetAllCategoriesAsync(
            PaginationParams pagination,
            string? search = null,
            string? sortBy = "name",
            string? sortOrder = "asc"
            )
        {
            var (categories, totalRecords) = await _categoryRepository.GetAllCategoriesAsync(pagination, search, sortBy, sortOrder);

            if (categories == null) throw new NotFoundException("Categories not found");

            var categoriesDtos = categories.Select(c => MapToCategoryResponseDto(c)).ToList();
            return new PagedResult<CategoryResponseDto>
            {
                Data = categoriesDtos,
                TotalRecords = totalRecords,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<CategoryResponseDto> UpdateCategoryAsync(Guid uid, CategoryRequestDto dto)
        {
            var categoryName = dto.Name.Trim();

            var existCat = await _categoryRepository.GetCategoryByIdAsync(uid);
            if (existCat == null)
                throw new NotFoundException("Category with this Guid not exist");

            if (dto.ParentId.HasValue && dto.ParentId.Value == existCat.Id)
                throw new ValidationException("Category cannot be its own parent.");

            if (dto.ParentId.HasValue)
            {
                var parentExist = await _categoryRepository.GetCategoryByPkAsync(dto.ParentId.Value);
                if (parentExist == null)
                    throw new NotFoundException($"Parent category with id {dto.ParentId.Value} not found.");
            }
            if (categoryName != null)
            {
                var exists = await _categoryRepository.CategoryExistsAsync(categoryName, dto.ParentId, uid);
                if (exists)
                    throw new ConflictException($"Category '{categoryName}' already exists.");
                var existingSlugs = await _categoryRepository.GetAllCategorySlugAsync() ?? new List<string>();
                existCat.Name = categoryName ?? existCat.Name;
                existCat.Slug = _slugService.GenerateUnique(categoryName, existingSlugs);
            }

            // Update fields
            existCat.Description = dto.Description?.Trim() ?? existCat.Description;
            existCat.ParentId = dto.ParentId ?? existCat.ParentId;

            await _categoryRepository.SaveChnagesAsync();

            return MapToCategoryResponseDto(existCat);
        }
        public async Task<bool> DeleteCategoryAsync(Guid uid)
        {
            var result = await _categoryRepository.DeleteCategoryAsync(uid);

            if (result == true) return true;

            return false;
        }

        private static CategoryResponseDto MapToCategoryResponseDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Uid = category.Uid,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ParentId = category.ParentId,
                //ParentName = parentExist?.Name,
                //PostCount = 0,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }
    }
}
