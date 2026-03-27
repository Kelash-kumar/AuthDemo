namespace AuthDemo.DTOs.CommentDtos
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string Uid { get; set; } = string.Empty;
        public int PostId { get; set; }
        public int? UserId { get; set; }
        public string Body { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public List<CommentResponseDto>? Replies { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }

}
