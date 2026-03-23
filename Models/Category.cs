namespace AuthDemo.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int? ParentId { get; set; }


        //Navigation properties
        public Category? Parent { get; set; } // Self-referencing relationship
        public ICollection<Post>? Posts { get; set; } // One-to-many relationship with Post
        public ICollection<Category>? Subcategories { get; set; } // One-to-many relationship with itself
    }
}
