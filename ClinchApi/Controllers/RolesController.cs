﻿using ClinchApi.Models;
using ClinchApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Gets all the roles as a list of string
    /// </summary>
    /// <returns>A list of string</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Get a role by its ID
    /// </summary>
    /// <param name="roleId">ID of the role to get</param>
    /// <returns>A role</returns>
    [HttpGet("{roleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoleById(string roleId)
    {
        try
        {
            var role = await _roleService.GetRoleById(roleId);

            return Ok(role);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Get the roles of a user
    /// </summary>
    /// <param name="userId">Id of the user whose roles will be returned</param>
    /// <returns>A list of string</returns>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        try
        {
            var roles = await _roleService.GetUserRolesAsync(userId);
            if (roles == null || roles.Count == 0)
            {
                return NotFound("No roles found for this user");
            }
            return Ok(roles);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Creates a new role
    /// </summary>
    /// <param name="roleName">Name of the role to be created</param>
    /// <returns>Ok result</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return BadRequest("Role name cannot be empty.");
        }

        try
        {
            var role = await _roleService.CreateRoleAsync(roleName);

            return role is not null ?
                CreatedAtAction(nameof(GetRoleById), new { roleId = role.Id.ToString() }, role) :
                BadRequest("Role creation failed");
        }
        catch (ArgumentException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Updates the name of a role
    /// </summary>
    /// <param name="roleId">ID of the role to be updated</param>
    /// <param name="newRoleName">The new role name</param>
    /// <returns>No content</returns>
    [HttpPut("{roleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRole(string roleId, [FromBody] string newRoleName)
    {
        if (string.IsNullOrWhiteSpace(newRoleName))
        {
            return BadRequest("New role name cannot be empty.");
        }

        try
        {
            if (await _roleService.UpdateRoleAsync(roleId, newRoleName))
            {
                return NoContent();
            }
            return NotFound($"Role with ID {roleId} not found.");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Deletes a role
    /// </summary>
    /// <param name="roleId">ID of the role to be deleted</param>
    /// <returns>No content</returns>
    [HttpDelete("{roleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        try
        {
            var roleIsDeleted = await _roleService.DeleteRoleAsync(roleId);
            if (roleIsDeleted)
            {
                return NoContent();
            }
            return NotFound($"Role with ID {roleId} not found.");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Assigns a role to a user
    /// </summary>
    /// <param name="model">A model containing the user id and the role name</param>
    /// <returns>Ok object result</returns>
    [HttpPost("assign")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
        {
            return BadRequest("User ID and role name cannot be empty.");
        }

        try
        {
            var result = await _roleService.AssignRoleToUserAsync(model.UserId, model.RoleName);
            if (result)
            {
                return Ok("Role assigned to user successfully.");
            }
            return BadRequest("Error assigning role to user.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Unassigns a role from a user
    /// </summary>
    /// <param name="model">A model containing the user id and the role name</param>
    /// <returns>No content</returns>
    [HttpPost("unassign")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnassignRoleFromUser([FromBody] AssignRoleModel model)
    {
        if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
        {
            return BadRequest("User ID and role name cannot be empty.");
        }

        try
        {
            var result = await _roleService.UnassignRoleFromUserAsync(model.UserId, model.RoleName);
            if (result)
            {
                return NoContent();
            }
            return BadRequest("Error unassigning role from user.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }
}
