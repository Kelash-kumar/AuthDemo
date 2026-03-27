using AuthDemo.DTOs.RoleDTOs;
using AuthDemo.Helpers;
using AuthDemo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    public class RoleController(IRoleService service) : BaseApiController
    {
        private readonly IRoleService _roleService = service;

        [HttpGet]
        public async Task<IActionResult> GetAllRoles(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = "name",
            [FromQuery] string? sortDirection = "asc"
            )
        {
            var result = await _roleService.GetAllRoles(paginationParams, search, sortBy, sortDirection);
            return ApiOk(result, "Roles Fetched Successfully.");
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid uid)
        {
            var result = await _roleService.GetRoleById(uid);
            return ApiOk(result, "Roles Fetched Successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleRequestDto roleRequestDto)
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

            var result = await _roleService.Create(roleRequestDto);
            return ApiCreated(result, "Role Created Successfulyy.");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{uid}")]
        public async Task<IActionResult> Update([FromRoute] Guid uid, [FromBody] RoleRequestDto roleRequestDto)
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

            var result = await _roleService.Update(uid, roleRequestDto);
            return ApiOk(result, "Role Updated Successfully.");
        }
    }
}
