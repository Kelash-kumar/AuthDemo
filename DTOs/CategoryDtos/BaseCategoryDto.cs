
namespace AuthDemo.DTOs.CategoryDtos
{
    public abstract class BaseCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentId { get; set; }
    }
}
