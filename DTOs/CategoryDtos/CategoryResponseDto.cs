using AuthDemo.Common;

namespace AuthDemo.DTOs.CategoryDtos
{
    public class CategoryResponseDto : BaseCategoryDto, IBaseResponseDto
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Slug { get; set; } = string.Empty;

        //public string? ParentName { get; set; }
        //public int PostCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
