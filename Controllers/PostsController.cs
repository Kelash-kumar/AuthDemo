using AuthDemo.DTOs.PostDtos;
using AuthDemo.Helpers;
using AuthDemo.Services.Implementations;
using AuthDemo.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{

    public class PostsController : BaseApiController
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "desc"

            )
        {
            var result = await _postService.GetAllPostsAsync(paginationParams, search, sortBy, sortDirection);

            return ApiOk(result, "Categories Fecthed Successfulyy.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDto dto)
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

            var result = await _postService.CreatePost(dto);

            return ApiOk(result ,"Post Created Successfully");
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid uid)
        {
            var category = await _postService.GetPostByIdAsync(uid);
            return ApiOk(category, "Category fetched successfully");
        }

    }
}
