namespace AuthDemo.DTOs.PostDtos
{
    public class PostResponseDto : BasePostDto
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Nested author & category info
        public AuthorDto? Author { get; set; }
        public CategoryDto? Category { get; set; }
    }

    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}


