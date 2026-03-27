using System.ComponentModel.DataAnnotations;

namespace AuthDemo.DTOs.TagsDtos
{
    public class TagRequestDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int? PostId { get; set; }

    }
}
