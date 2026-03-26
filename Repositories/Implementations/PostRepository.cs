using AuthDemo.Data;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Repositories.Implementations
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Post> CreatePost(Post post)
        {
            var newPost = await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return await Task.FromResult(newPost.Entity);

        }

        public async Task<(List<Post>, int totalRecords)> GetAllPostsAsync(
        PaginationParams paginationParams,
        string search = null,
        string sortBy = "createdAt",
        string sortDirection = "desc")
        {
            var query = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(p =>
                    p.Title.ToLower().Contains(term) ||
                    p.Excerpt.ToLower().Contains(term) ||
                    p.Content.ToLower().Contains(term));
            }

            // Sorting
            var isDesc = sortDirection.Trim().Equals("desc", StringComparison.OrdinalIgnoreCase);
            query = sortBy?.Trim().ToLower() switch
            {
                "title" => isDesc ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
                "createdat" => isDesc ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                _ => isDesc ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
            };

            // Pagination
            var totalRecords = await query.CountAsync();
            var posts = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return (posts, totalRecords);
        }

        public async Task<List<string>> GetAllPostSlugsAsync()
        {
            return await _context.Posts.Select(p => p.Slug).ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid uid)
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Uid == uid);
        }
    }
}
