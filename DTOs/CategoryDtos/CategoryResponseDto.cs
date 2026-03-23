namespace AuthDemo.DTOs.CategoryDtos
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        //public string? ParentName { get; set; }
        //public int PostCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
