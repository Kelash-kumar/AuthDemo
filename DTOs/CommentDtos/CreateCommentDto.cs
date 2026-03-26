namespace AuthDemo.DTOs.CommentDtos
{
    public class CreateCommentDto
    {
        public int PostId { get; set; }

        public int? UserId { get; set; }

        public string? GuestName { get; set; }
        public string? GuestEmail { get; set; }

        public string Body { get; set; } = string.Empty;

        public int? ParentId { get; set; }
    }
}
