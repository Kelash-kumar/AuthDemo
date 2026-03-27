using AuthDemo.DTOs.TagsDtos;
using AuthDemo.Helpers;

namespace AuthDemo.Services.Interfaces
{
    public interface ITagService
    {
        public Task<PagedResult<TagResponseDto>> GetAllTagssAsync(
        PaginationParams pagination,
        string? search = null,
        string? sortBy = "name",
        string? sortOrder = "asc"
        );
        public Task<TagResponseDto> GetTagByIdAsync(Guid id);
        public Task<TagResponseDto> GetTagBySlugAsync(string slug);
        public Task<TagResponseDto> CreateTagAsync(TagRequestDto dto);
        public Task<TagResponseDto> UpdateTagAsync(Guid uid, TagRequestDto dto);
        public Task<bool> DeleteTagAsync(Guid id);
    }
}
