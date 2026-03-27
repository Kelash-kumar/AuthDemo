using AuthDemo.DTOs.TagsDtos;
using AuthDemo.Helpers;
using AuthDemo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{
    [Authorize]
    public class TagsController : BaseApiController
    {
        private readonly ITagService _tagService;
        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "asc"
            )
        {
            var result = await _tagService.GetAllTagssAsync(paginationParams, search, sortBy, sortDirection);

            return ApiOk(result);
        }


        [HttpGet("{uid}")]
        public async Task<IActionResult> GetTagById([FromRoute] Guid uid)
        {
            var result = await _tagService.GetTagByIdAsync(uid);
            return ApiOk(result, "Tags Fecthed Successfulyy.");
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetTagBySlug([FromRoute] string slug)
        {
            var result = await _tagService.GetTagBySlugAsync(slug);
            return ApiOk(result, "Tags Fecthed Successfulyy.");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TagRequestDto dto)
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

            var result = await _tagService.CreateTagAsync(dto);

            return ApiCreated(result, "Tag Created Successfulyy.");
        }


        [HttpPut("{uid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid uid, [FromBody] TagRequestDto dto)
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
            var result = await _tagService.UpdateTagAsync(uid, dto);

            return ApiOk(result, "Tag Updated Successfulyy.");
        }

        [HttpDelete("{uid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid uid)
        {
            var result = await _tagService.DeleteTagAsync(uid);

            return ApiOk(result, "Tags Fecthed Successfulyy.");
        }


    }
}
