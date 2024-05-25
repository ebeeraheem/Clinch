﻿using ClinchApi.Entities;
using ClinchApi.Models;
using ClinchApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly UserService _userService;

    public AccountsController(UserService userService)
    {
        _userService = userService;
    }

    // Get all users
    [HttpGet]
    [Authorize]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    // Get user by ID
    [HttpGet("{userId}")]
    [Authorize]
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

    [HttpPost("update")]
    [Authorize]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(System.Security.Claims
            .ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _userService.UpdateUserAsync(userId, model);
        if (result.Succeeded)
        {
            return NoContent();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return BadRequest(ModelState);
    }

    // Log out
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await _userService.LogOutAsync();
        return NoContent();
    }

    // Delete user account
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _userService.DeleteUserAsync(id);
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


    //private readonly UserManager<ApplicationUser> _userManager;
    //private readonly SignInManager<ApplicationUser> _signInManager;

    //public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    //{
    //    _userManager = userManager;
    //    _signInManager = signInManager;
    //}

    //[HttpPost("register")]
    //public async Task<IActionResult> Register([FromBody] RegisterModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    var user = new ApplicationUser
    //    {
    //        UserName = model.Email,
    //        Email = model.Email,
    //        FirstName = model.FirstName,
    //        LastName = model.LastName,
    //        Gender = model.Gender,
    //        DateOfBirth = model.DateOfBirth,
    //        //UserAddressId = model.UserAddressId
    //    };

    //    var result = await _userManager.CreateAsync(user, model.Password);

    //    if (result.Succeeded)
    //    {
    //        // Additional logic after successful registration, e.g., sign in the user or send a confirmation email.
    //        await _signInManager.SignInAsync(user, isPersistent: false);
    //        return Ok();
    //    }

    //    foreach (var error in result.Errors)
    //    {
    //        ModelState.AddModelError(string.Empty, error.Description);
    //    }

    //    return BadRequest(ModelState);
    //}
}
