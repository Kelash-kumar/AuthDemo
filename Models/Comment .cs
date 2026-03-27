namespace AuthDemo.Models
{
    public class Comment : BaseEntity
    {
        public int PostId { get; set; }
        public int? UserId { get; set; } // Nullable for guest comments
        public int? ParentId { get; set; } // For nested comments

        public string Body { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;

        //Guest comment fields
        public string? GuestName { get; set; }
        public string? GuestEmail { get; set; }

        // Navigation properties
        public Post Post { get; set; } = null!;
        public User? User { get; set; } // Nullable for guest comments
        public Comment? Parent { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
