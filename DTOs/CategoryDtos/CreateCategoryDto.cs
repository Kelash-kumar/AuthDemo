using System.ComponentModel.DataAnnotations;

namespace AuthDemo.DTOs.CategoryDtos
{
    public class CreateCategoryDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public int? ParentId { get; set; }  
    }
}
