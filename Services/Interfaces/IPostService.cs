using AuthDemo.DTOs.PostDtos;
using AuthDemo.Helpers;

namespace AuthDemo.Services.Interfaces
{
    public interface IPostService
    {
        Task<PostResponseDto> CreatePost(CreatePostDto postDto);
        Task<PostResponseDto> GetPostByIdAsync(Guid uid);
        Task<PagedResult<PostResponseDto>> GetAllPostsAsync(
            PaginationParams pagination,
            string? search = null,
            string? sortBy = "createdAt",
            string? sortOrder = "asc"
            );
    }
}
