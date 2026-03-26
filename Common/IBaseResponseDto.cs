namespace AuthDemo.Common
{
    public interface IBaseResponseDto
    {
        public int Id { get; set; }

        public Guid Uid { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
