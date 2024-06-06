using Microsoft.AspNetCore.Identity;

namespace ClinchApi.Services.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AssignRoleToUserAsync(string userId, string roleName);
        Task<IdentityRole<int>> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<List<string?>> GetAllRolesAsync();
        Task<IdentityRole<int>> GetRoleById(string roleId);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> UnassignRoleFromUserAsync(string userId, string roleName);
        Task<bool> UpdateRoleAsync(string roleId, string newRoleName);
    }
}