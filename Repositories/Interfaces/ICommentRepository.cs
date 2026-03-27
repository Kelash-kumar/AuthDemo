using AuthDemo.Helpers;
using AuthDemo.Models;

namespace AuthDemo.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Guid uid, Comment comment);
        Task<Comment> GetCommentByIdAsync(Guid uid);
        Task<(List<Comment>, int totalRecords)> GetCommentByIdAsync(
            PaginationParams paginationParams,
            string? search = null,
            string sortBy = "createdAt",
            string sortDirection = "asc"
            );
        Task<Comment> GetCommentByPksync(int Id);
        Task<List<Comment>> GetCommentsByPostIdAsync(int Id);
    }
}
