using AuthDemo.Data;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Repositories.Implementations
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<int>> GetValidRoleIdsAsync(List<int> roleIds)
        {
            return await _context.Roles
        .Where(r => roleIds.Contains(r.Id))
        .Select(r => r.Id)
        .ToListAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<Role>> GetRolesByIdsAsync(List<int> roleIds)
        {
            return await _context.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
        }


        public async Task<(List<User>, int TotalRecords)> GetAllUsers(
            PaginationParams paginationParams,
            string? search = null,
            string? sortBy = "name",
            string? sortDirection = "asc"
        )
        {
            if (paginationParams.PageNumber < 1) paginationParams.PageNumber = 1;
            if (paginationParams.PageSize < 1) paginationParams.PageSize = 10;

            var query = _context.Users
                .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.Name.Contains(search) ||
                    u.Email.Contains(search) ||
                    (u.Bio != null && u.Bio.Contains(search))
                );
            }

            // Sorting
            var isDesc = sortDirection?.ToLower() == "desc";

            query = sortBy?.ToLower() switch
            {
                "createdat" => isDesc
                    ? query.OrderByDescending(u => u.CreatedAt)
                    : query.OrderBy(u => u.CreatedAt),

                "name" => isDesc
                    ? query.OrderByDescending(u => u.Name)
                    : query.OrderBy(u => u.Name),

                _ => isDesc
                    ? query.OrderByDescending(u => u.Id)
                    : query.OrderBy(u => u.Id)
            };

            var totalRecords = await query.CountAsync();

            var users = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return (users, totalRecords);
        }

        public async Task<User> UpdateUserAsync(Guid uid, User user)
        {
            var affected = await _context.Users
                .Where(u => u.Uid == uid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.Name, user.Name)
                    .SetProperty(u => u.Bio, user.Bio)
                    .SetProperty(u => u.Avatar, user.Avatar)
                );

            if (affected == 0)
            {
                return null;
            }

            return await _context.Users.FirstOrDefaultAsync(u => u.Uid == uid);
        }

        public async Task<User> GetUserByIdAsync(Guid uid)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Uid == uid);
        }
    }
}
