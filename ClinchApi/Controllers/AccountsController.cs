using ClinchApi.Extensions;
using ClinchApi.Models;
using ClinchApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountsController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>Returns a list of all users</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    /// <summary>
    /// Return a user with the specified ID
    /// </summary>
    /// <param name="userId">ID of the user to return</param>
    /// <returns>A user</returns>
    [AuthorizeUserOrAdmin]
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(string userId)
    {
        try
        {
            var userModel = await _userService.GetUserByIdAsync(userId);
            return Ok(userModel);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occured");
        }
    }

    /// <summary>
    /// Updates a user's profile
    /// </summary>
    /// <param name="model">A model that contsind the updated user details</param>
    /// <returns>No content</returns>
    [AuthorizeUserOrAdmin]
    [HttpPost("{userId}/update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserProfile(string userId, [FromBody] UpdateUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.UpdateUserAsync(userId, model);
        if (result.Succeeded)
        {
            return NoContent();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("Error", error.Description);
        }
        return BadRequest(ModelState);
    }

    /// <summary>
    /// Logs a user out
    /// </summary>
    /// <returns>No content</returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> LogOut()
    {
        await _userService.LogOutAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a user's account
    /// </summary>
    /// <param name="id">ID of the user whose account is to be deleted</param>
    /// <returns>No content</returns>
    [AuthorizeUserOrAdmin]
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _userService.DeleteUserAsync(userId);
        if (result.Succeeded)
        {
            return NoContent();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("Error", error.Description);
        }
        return BadRequest(ModelState);
    }
}
