using AuthDemo.DTOs.CommentDtos;
using AuthDemo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{
    [Authorize]
    public class CommentsController : BaseApiController
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> GetComment([FromRoute] Guid uid)
        {

            var result = await _commentService.GetCommentByIdAsync(uid);

            return ApiOk(result);
        }

        //Get All Comments replies of post
        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetCommentsWithRepliesAsync([FromRoute] int id)
        {

            var result = await _commentService.GetCommentsWithRepliesAsync(id);

            return ApiOk(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto dto)
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

            var result = await _commentService.CreateCommentAsync(dto);

            return ApiCreated(result, "Comment Created Successfully.");
        }

        [HttpPut("{uid}")]
        public async Task<IActionResult> Update([FromRoute] Guid uid, [FromBody] UpdateCommentDto dto)
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

            var result = await _commentService.UpdateCommentAsync(uid, dto);

            return ApiOk(result, "Comment Updated Successfully.");
        }
    }
}
