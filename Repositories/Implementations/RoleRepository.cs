using AuthDemo.Data;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Repositories.Implementations
{
    public class RoleRepository(ApplicationDbContext context) : IRoleRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<(List<Role>, int TotalRecords)> GetAllRolesAsync(
             PaginationParams paginationParams,
            string? search = null,
            string? sortBy = "name",
            string? sortDirection = "asc")
        {
            var query = _context.Roles.AsQueryable();

            //seach filter
            if (!string.IsNullOrEmpty(search))
                query = query.Where(r => r.Name.Contains(search) || r.Description.Contains(search));

            query = sortBy?.ToLower() switch
            {
                "id" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(r => r.Id) : query.OrderBy(r => r.Id),
                "name" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
                "description" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(r => r.Description) : query.OrderBy(r => r.Description),
                "createdat" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt),
                _ => query.OrderBy(r => r.Name)
            };

            int totalRecords = await query.CountAsync();
            var roles = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return (roles, totalRecords);
        }
        public async Task<Role> GetRolesByIdsAsync(Guid uid)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Uid == uid);
        }
        public async Task<Role> GetRoleByNameAsync(String name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
        public async Task CreateRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public async Task<Role?> UpdateRoleAsync(Guid uid, Role role)
        {
            var affected = await _context.Roles
                .Where(r => r.Uid == uid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(r => r.Name, role.Name)
                    .SetProperty(r => r.Description, role.Description)
                );

            if (affected == null)
            {
                return null;
            }

            return await _context.Roles.FirstOrDefaultAsync(r => r.Uid == uid);

        }
    }
}
