using ClinchApi.Data;
using ClinchApi.Entities;
using ClinchApi.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinchApi.Services;

public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    //Get user by id/guid

    // Get all users (optional parameters)

    // Update user details
    public async Task UpdateProfile(string userId, UpdateUserModel user)
    {
        //Find user by guid
        var userToUpdate = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"User with ID {userId} not found");

        //userToUpdate.FirstName = user.FirstName;
        //userToUpdate.MiddleName = user.MiddleName;
        //userToUpdate.LastName = user.LastName;
        //userToUpdate.Email = user.Email;
        //userToUpdate.PhoneNumber = user.PhoneNumber;
        //userToUpdate.Gender = user.Gender;
        //userToUpdate.DateOfBirth = user.DateOfBirth;

        await _userManager.UpdateAsync(userToUpdate);
    }

    // Log out

    // Delete user account


    //ENDPOINTS MANAGED BY IDENTITY
    // - register, login, refresh token
    // - confirm email, resend confirmation email
    // - forgot password, reset password, change password (POST/manage/info)
    // - get logged-in user info (GET/manage/info)
}
