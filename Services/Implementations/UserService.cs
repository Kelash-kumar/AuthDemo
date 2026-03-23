using AuthDemo.DTOs.UserDTOs;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using AuthDemo.Services.Interfaces;

namespace AuthDemo.Services.Implementations
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<PagedResult<UserResponseDto>> GetAllUsers(
            PaginationParams pagination,
            string? search = null,
            string? sortBy = "name",
            string? sortDirection = "asc")
        {
            if (pagination.PageNumber < 1) pagination.PageNumber = 1;
            if (pagination.PageSize < 1) pagination.PageSize = 10;

            var (users, totalRecords) = await _userRepository
                .GetAllUsers(pagination, search, sortBy, sortDirection);

            users ??= new List<User>();

            var userDtos = users.Select(MapToUserResponseDto).ToList();

            return new PagedResult<UserResponseDto>
            {
                Data = userDtos,
                TotalRecords = totalRecords,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<UserResponseDto> UpdateUserAsync(Guid uid, UserRequestDto userUpdateDto)
        {

            var updatedUser = await _userRepository.UpdateUserAsync(uid, new User
            {
                Name = userUpdateDto.Name,
                Bio = userUpdateDto.Bio,
                Avatar = userUpdateDto.ProfilePictureUrl
            });

            return MapToUserResponseDto(updatedUser);

        }

        public async Task<UserResponseDto> GetUserByIdAsync(Guid uid)
        {

            var user = await _userRepository.GetUserByIdAsync(uid);

            return MapToUserResponseDto(user);
        }
        private static UserResponseDto MapToUserResponseDto(User user) => new()
        {
            Id = user.Id,
            Uuid = user.Uid.ToString(),
            Name = user.Name,
            Email = user.Email,
            Bio = user.Bio,
            AvatarUrl = user.Avatar,
            Status = user.Status,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList()
        };


    }
}