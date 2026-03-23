namespace AuthDemo.DTOs.UserDTOs
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string>? Roles { get; set; }
        public string token { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
