using AuthDemo.DTOs.CommentDtos;

namespace AuthDemo.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto dto);
        Task<CommentResponseDto> UpdateCommentAsync(Guid uid, UpdateCommentDto dto);
        Task<CommentResponseDto> GetCommentByIdAsync(Guid uid);
        public Task<List<CommentResponseDto>> GetCommentsWithRepliesAsync(int PostId);


    }
}
