namespace AuthDemo.DTOs.PostDtos
{
    public abstract class BasePostDto
    {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string FeaturedImage { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
    }
}
