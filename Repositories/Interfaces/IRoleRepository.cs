using AuthDemo.Helpers;
using AuthDemo.Models;

namespace AuthDemo.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<(List<Role>, int TotalRecords)> GetAllRolesAsync(
            PaginationParams paginationParams,
            string? search = null,
            string? sortBy = "name",
            string? sortDirection = "asc"
            );
        Task<Role> GetRolesByIdsAsync(Guid uid);
        Task<Role> GetRoleByNameAsync(string name);
        Task CreateRoleAsync(Role role);
        Task<Role?> UpdateRoleAsync(Guid uid, Role role);
        //Task DeleteRoleAsync(Guid uid);

    }
}
