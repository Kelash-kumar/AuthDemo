namespace AuthDemo.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int PostId { get; set; }

        // Navigation property
        public ICollection<PostTags> PostTags { get; set; } = new List<PostTags>();
    }
}