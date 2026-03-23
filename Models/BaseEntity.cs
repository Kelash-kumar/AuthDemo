namespace AuthDemo.Models
{
    public class BaseEntity
    {
        public int Id { get; set; } // Primary key, auto-incremented
        public Guid Uid { get; set; } = Guid.NewGuid(); // Unique identifier for the user
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
