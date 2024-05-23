﻿using ClinchApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RoleService _roleService;

    public RolesController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var roles = await _roleService.GetAllRolesAsync();
            if (roles == null || roles.Count == 0)
            {
                return NotFound("No roles found");
            }
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        try
        {
            var roles = await _roleService.GetUserRolesAsync(userId);
            if (roles == null || !roles.Any())
            {
                return NotFound("No roles found for this user");
            }
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return BadRequest("Role name cannot be empty.");
        }

        if (await _roleService.CreateRoleAsync(roleName))
        {
            return Ok(new { message = "Role created successfully." });
        }
        return StatusCode(StatusCodes.Status500InternalServerError, "Error creating role.");
    }

    [HttpPut("{roleId}")]
    public async Task<IActionResult> UpdateRole(string roleId, [FromBody] string newRoleName)
    {
        if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(newRoleName))
        {
            return BadRequest("ID and new role name cannot be empty.");
        }

        if (await _roleService.UpdateRoleAsync(roleId, newRoleName))
        {
            return Ok(new { message = "Role updated successfully." });
        }
        return NotFound("Role not found.");
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        if (string.IsNullOrEmpty(roleId))
        {
            return BadRequest("Role ID cannot be empty.");
        }

        var roleExists = await _roleService.DeleteRoleAsync(roleId);
        if (roleExists)
        {
            return Ok(new { message = "Role deleted successfully." });
        }
        return NotFound("Role not found.");
    }
        
    [HttpPost("assign")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
        {
            return BadRequest("User ID and role name cannot be empty.");
        }

        var result = await _roleService.AssignRoleToUserAsync(model.UserId, model.RoleName);
        if (result)
        {
            return Ok(new { message = "Role assigned to user successfully." });
        }
        return BadRequest("Error assigning role to user. The role might not exist or the user might already have the role.");
    }

    [HttpPost("unassign")]
    public async Task<IActionResult> UnassignRoleFromUser([FromBody] AssignRoleModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
        {
            return BadRequest("User ID and role name cannot be empty.");
        }

        var result = await _roleService.UnassignRoleFromUserAsync(model.UserId, model.RoleName);
        if (result)
        {
            return Ok(new { message = "Role unassigned from user successfully." });
        }
        return BadRequest("Error unassigning role from user. The user might not have the role.");
    }
}
