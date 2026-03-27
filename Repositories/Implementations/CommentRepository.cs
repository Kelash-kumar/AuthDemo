using AuthDemo.Data;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            var newComment = await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return newComment.Entity;
        }

        public async Task<Comment> GetCommentByIdAsync(Guid uid)
        {
            return await _context.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.Uid == uid);
        }

        public Task<(List<Comment>, int totalRecords)> GetCommentByIdAsync(PaginationParams paginationParams, string? search = null, string sortBy = "createdAt", string sortDirection = "asc")
        {
            throw new NotImplementedException();
        }

        public async Task<Comment> GetCommentByPksync(int Id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == Id);

        }

        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment> UpdateCommentAsync(Guid uid, Comment comment)
        {
            var affected = await _context.Comments
                .Where(c => c.Uid == uid)
                .ExecuteUpdateAsync(setters => setters
                 .SetProperty(c => c.Body, comment.Body)
                );

            if (affected == null) return null;

            return await GetCommentByIdAsync(uid);

        }
    }
}
