using AuthDemo.DTOs.UserDTOs;
using AuthDemo.Helpers;
using AuthDemo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "name",
            [FromQuery] string? sortDirection = "asc"
        )
        {
            var result = await _userService.GetAllUsers(
                paginationParams, search, sortBy, sortDirection
            );

            return ApiOk(result, "Users Fetched Successfully.");
        }


        [HttpGet("{uid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid uid)
        {
            var result = await _userService.GetUserByIdAsync(uid);
            return ApiOk(result, "User Fecthed Successfully");
        }

        [HttpPut("{uid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(Guid uid, [FromBody] UserRequestDto userUpdateDto)
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
            var result = await _userService.UpdateUserAsync(uid, userUpdateDto);
            return ApiOk(result, "User updated Successfully");
        }
    }
}