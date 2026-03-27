using AuthDemo.DTOs.RoleDTOs;
using AuthDemo.Exceptions;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using AuthDemo.Services.Interfaces;

namespace AuthDemo.Services.Implementations
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<RoleResponseDto> Create(RoleRequestDto roleRequestDto)
        {
            var existingRole = await _roleRepository.GetRoleByNameAsync(roleRequestDto.Name);

            if (existingRole != null)
            {
                throw new ConflictException($"Role with name '{roleRequestDto.Name}' already exists.");
            }

            var role = new Role
            {
                Uid = Guid.NewGuid(),
                Name = roleRequestDto.Name,
                Description = roleRequestDto.Description,
            };

            await _roleRepository.CreateRoleAsync(role);

            var res = new RoleResponseDto
            {
                Id = role.Id,
                Uuid = role.Uid.ToString(),
                Name = role.Name,
                CreatedAt = role.CreatedAt
            };
            return res;
        }

        public async Task<PagedResult<RoleResponseDto>> GetAllRoles(
            PaginationParams pagination,
            string? search,
            string? sortBy,
            string? sortOrder)
        {

            var (roles, totalRecords) = await _roleRepository.GetAllRolesAsync(pagination, search, sortBy, sortOrder);

            //Map role => DTOs
            var roleDtos = roles.Select(r => MapToRoleResponseDto(r)).ToList();

            return new PagedResult<RoleResponseDto>
            {
                Data = roleDtos,
                TotalRecords = totalRecords,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<RoleResponseDto> GetRoleById(Guid uid)
        {
            var role = await _roleRepository.GetRolesByIdsAsync(uid);
            return role == null
                ? throw new NotFoundException($"Role with ID '{uid}' not found.")
                : role is null ? null : MapToRoleResponseDto(role);
        }

        public async Task<RoleResponseDto> Update(Guid uid, RoleRequestDto roleRequestDto)
        {
            var updatedRole = await _roleRepository.UpdateRoleAsync(uid, new Role
            {
                Name = roleRequestDto.Name,
                Description = roleRequestDto.Description
            });

            return updatedRole == null ? throw new NotFoundException($"Role with ID {uid} not found.") : MapToRoleResponseDto(updatedRole);
        }

        private static RoleResponseDto MapToRoleResponseDto(Role role)
        {
            return new RoleResponseDto
            {
                Id = role.Id,
                Uuid = role.Uid.ToString(),
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt
            };
        }
    }
}
