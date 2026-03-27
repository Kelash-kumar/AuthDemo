using AuthDemo.DTOs.TagsDtos;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using AuthDemo.Services.Interfaces;


namespace AuthDemo.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly ISlugService _slugService;
        public TagService(ITagRepository tagRepository, ISlugService slugService)
        {
            _tagRepository = tagRepository;
            _slugService = slugService;
        }

        public async Task<TagResponseDto> CreateTagAsync(TagRequestDto dto)
        {
            var tagName = dto.Name.Trim();

            var existingSlugs = await _tagRepository.GetAllTagSlugAsync() ?? new List<string>();
            var slug = _slugService.GenerateUnique(dto.Name, existingSlugs);

            var tag = new Tag
            {
                Name = dto.Name,
                Slug = slug,
                //PostId = (int) dto.PostId
            };

            var newTag = await _tagRepository.CreateTagAsync(tag);
            await _tagRepository.SaveChangesAsync();

            return MapTagResponseDto(newTag);
        }

        public async Task<bool> DeleteTagAsync(Guid id)
        {
            return await _tagRepository.DeleteTagAsync(id);
        }

        public async Task<PagedResult<TagResponseDto>> GetAllTagssAsync(
            PaginationParams paginationParams,
            string? search = null,
            string? sortBy = "name",
            string? sortDirection = "asc"
            )
        {
            var (tags, totalRecords) = await _tagRepository.GetAllTagsAsync(paginationParams, search, sortBy, sortDirection);
            var tagDtos = tags.Select(t => MapTagResponseDto(t)).ToList();

            return new PagedResult<TagResponseDto>
            {
                Data = tagDtos,
                TotalRecords = totalRecords,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };
        }

        public async Task<TagResponseDto> GetTagByIdAsync(Guid id)
        {
            var result = await _tagRepository.GetTagByIdAsync(id);
            return MapTagResponseDto(result);
        }

        public async Task<TagResponseDto> GetTagBySlugAsync(string slug)
        {
            var result = await _tagRepository.GetTagBySlugAsync(slug);
            return MapTagResponseDto(result);
        }

        public async Task<TagResponseDto> UpdateTagAsync(Guid uid, TagRequestDto dto)
        {
            var tagName = dto.Name.Trim();

            var existingSlugs = await _tagRepository.GetAllTagSlugAsync() ?? new List<string>();
            var slug = _slugService.GenerateUnique(dto.Name, existingSlugs);

            var tag = new Tag
            {
                Name = dto.Name,
                Slug = slug,
            };

            var updatedTag = await _tagRepository.UpdateTagAsync(uid, tag);

            return MapTagResponseDto(updatedTag);
        }

        private static TagResponseDto MapTagResponseDto(Tag tag)
        {
            return new TagResponseDto
            {
                Id = tag.Id,
                Uid = tag.Uid,
                Name = tag.Name,
                Slug = tag.Slug,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt
            };
        }


    }
}
