using ClinchApi.Data;
using ClinchApi.Entities;
using ClinchApi.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinchApi.Services;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    //Get user by id/guid

    // Get all users (optional parameters)

    // Update user details
    public async Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserModel model)
    {
        //Find user by guid
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return IdentityResult.Failed(
                new IdentityError { Description = "User not found" });
        }        

        user.FirstName = model.FirstName;
        user.MiddleName = model.MiddleName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.UserName = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.Gender = model.Gender;
        user.DateOfBirth = model.DateOfBirth;

        return await _userManager.UpdateAsync(user);
    }

    // Log out

    // Delete user account


    //ENDPOINTS MANAGED BY IDENTITY
    // - register, login, refresh token
    // - confirm email, resend confirmation email
    // - forgot password, reset password, change password (POST/manage/info)
    // - get logged-in user info (GET/manage/info)
}
