using System.ComponentModel.DataAnnotations;

namespace AuthDemo.DTOs.UserDTOs
{
    public class UserRequestDto
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }
        [StringLength(100, MinimumLength = 10)]
        public string? Bio { get; set; }
        [Url]
        public string? ProfilePictureUrl { get; set; }
    }
}
