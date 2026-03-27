using AuthDemo.Helpers;
using AuthDemo.Models;

namespace AuthDemo.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> CreatePost(Post post);
        Task<(List<Post>, int totalRecords)> GetAllPostsAsync(
            PaginationParams paginationParams,
            string search,
            string sortBy,
            string sortDirection
            );
        Task<List<string>> GetAllPostSlugsAsync();
        Task<Post> GetPostByIdAsync(Guid uid);
    }
}
