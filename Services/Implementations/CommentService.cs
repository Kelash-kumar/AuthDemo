using AuthDemo.DTOs.CommentDtos;
using AuthDemo.Exceptions;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using AuthDemo.Services.Interfaces;
namespace AuthDemo.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto dto)
        {
            int? parentCommentId = dto.ParentId;

            // Validate parent comment
            if (parentCommentId.HasValue)
            {
                var parentComment = await _commentRepository.GetCommentByPksync(parentCommentId.Value);

                if (parentComment == null)
                    throw new NotFoundException("Parent Comment does not exist.");
            }

            // Validate user or guest
            if (dto.UserId == null && string.IsNullOrEmpty(dto.GuestName))
            {
                throw new BadRequestException("User or Guest info is required.");
            }

            var newComment = new Comment
            {
                PostId = dto.PostId,
                UserId = dto.UserId,
                Body = dto.Body,
                ParentId = parentCommentId,
                GuestEmail = dto.GuestEmail,
                GuestName = dto.GuestName,
                IsApproved = false // moderation
            };

            var commentCreated = await _commentRepository.CreateCommentAsync(newComment);

            return MapCommentResponseDto(commentCreated);
        }

        public async Task<CommentResponseDto> GetCommentByIdAsync(Guid uid)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(uid);
            return MapCommentResponseDto(comment);
        }

        public async Task<CommentResponseDto> UpdateCommentAsync(Guid uid, UpdateCommentDto dto)
        {
            // Update fields
            var comment = new Comment
            {
                Body = dto.Body,
            };

            var updatedComment = await _commentRepository.UpdateCommentAsync(uid, comment);

            if (updatedComment == null) throw new NotFoundException("comment not exist or Change not detected.");

            return MapCommentResponseDto(comment);
        }

        public async Task<List<CommentResponseDto>> GetCommentsWithRepliesAsync(int PostId)
        {
            var comments = await _commentRepository.GetCommentsByPostIdAsync(PostId);

            //convert to map dtos
            var commentsDtos = comments.Select(c => MapCommentResponseDto(c)).ToList();
            var lookup = commentsDtos.ToDictionary(c => c.Id);

            var rootComments = new List<CommentResponseDto>();

            foreach (var comment in commentsDtos)
            {
                if (comment.ParentId.HasValue && lookup.TryGetValue(comment.ParentId.Value, out CommentResponseDto? value))
                {
                    value.Replies.Add(comment);
                }
                else
                {
                    rootComments.Add(comment);
                }
            }

            return rootComments;
        }

        protected static CommentResponseDto MapCommentResponseDto(Comment comment)
        {
            return new CommentResponseDto
            {
                Id = comment.Id,
                Uid = comment.Uid.ToString(),
                PostId = comment.PostId,
                UserId = comment.UserId,
                Body = comment.Body,
                ParentId = comment.ParentId,
                IsApproved = comment.IsApproved,

                AuthorName = comment.User != null
                    ? comment.User.Name
                    : comment.GuestName ?? "Anonymous",

                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,

                Replies = comment.Replies?
                    .Select(r => MapCommentResponseDto(r))
                    .ToList() ?? new List<CommentResponseDto>()
            };
        }

    }
}
