using AuthDemo.DTOs.CategoryDtos;
using AuthDemo.Helpers;
using AuthDemo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{
    [Authorize]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //Get All Categories
        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "asc"

            )
        {
            var result = await _categoryService.GetAllCategoriesAsync(paginationParams, search, sortBy, sortDirection);

            return ApiOk(result, "Categories Fecthed Successfulyy.");
        }
        //POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CategoryRequestDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                                .Where(e => e.Value?.Errors.Count > 0)
                                .SelectMany(e => e.Value?.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();
                return ApiValidationError(errors);
            }

            var category = await _categoryService.CreateCategoryAsync(createCategoryDto);
            return ApiCreated(category, "Category created successfully");
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid uid)
        {
            var category = await _categoryService.GetCategoryByIdAsync(uid);
            return ApiOk(category, "Category fetched successfully");
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetCategoryBySlug([FromRoute] string slug)
        {
            var category = await _categoryService.GetCategoryBySlugAsync(slug);
            return ApiOk(category, "Category fetched successfully");
        }

        [HttpPut("{uid}")]
        public async Task<IActionResult> Update([FromRoute] Guid uid, [FromBody] CategoryRequestDto categoryRequestDto)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                                .Where(e => e.Value?.Errors.Count > 0)
                                .SelectMany(e => e.Value?.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();
                return ApiValidationError(errors);
            }

            var category = await _categoryService.UpdateCategoryAsync(uid, categoryRequestDto);
            return ApiOk(category, "Category Updated Successfully");
        }


        [HttpDelete("{uid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid uid)
        {
            await _categoryService.DeleteCategoryAsync(uid);
            return ApiOk("Category Deleted Sucessfully");
        }

    }
}
