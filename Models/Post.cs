namespace AuthDemo.Models
{
    public class Post : BaseEntity
    {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Excerpt { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string FeaturedImage { get; set; } = string.Empty;
        public PostStatus Status { get; set; } = PostStatus.Draft;
        public DateTime PublishedAt { get; set; }

        // Navigation properties
        public User Author { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        public ICollection<PostTags>? PostTags { get; set; } = new List<PostTags>();
    }

    public enum PostStatus
    {
        Draft,
        Published,
        Archived
    }
}
