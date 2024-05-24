using ClinchApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class RoleService
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(RoleManager<IdentityRole<int>> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    //Get all roles
    public async Task<List<string?>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles
            .Select(r => r.Name).ToListAsync();

        return roles;
    }

    //Get role by ID
    public async Task<IdentityRole<int>> GetRoleById(string roleId)
    {
        return await _roleManager.FindByIdAsync(roleId) ?? 
            throw new InvalidOperationException($"Role with ID {roleId} not found");
    }

    //Get the roles of a user
    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? 
            throw new InvalidOperationException($"User with ID {userId} not found");

        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    //Create a new role
    public async Task<IdentityRole<int>> CreateRoleAsync(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (roleExists)
        {
            throw new ArgumentException("Role already exists", roleName);            
        }

        var role = new IdentityRole<int>(roleName);
        
        await _roleManager.CreateAsync(role);

        return role;
    }

    //Update a role
    public async Task<bool> UpdateRoleAsync(string roleId, string newRoleName)
    {
        // Check if a role with the new role name already exists
        var existingRole = await _roleManager.FindByNameAsync(newRoleName);
        if (existingRole != null && existingRole.Id.ToString() != roleId)
        {
            throw new InvalidOperationException($"A role with the name '{newRoleName}' already exists.");
        }

        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is not null)
        {
            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);

            return result.Succeeded ?
                true :
                throw new InvalidOperationException($"Failed to update role {roleId}");
        }
        return false;
    }

    //Delete a role
    public async Task<bool> DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is not null)
        {
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
        return false;
    }

    //Assign role to a user
    public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            return false;
        }

        if (await _userManager.IsInRoleAsync(user, roleName))
        {
            return false; // User already has the role
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    //Unassign role from a user
    public async Task<bool> UnassignRoleFromUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return false;
        }

        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            return false; // User does not have the role
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        return result.Succeeded;
    }
}
