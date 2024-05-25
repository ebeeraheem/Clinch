using ClinchApi.Data;
using ClinchApi.Entities;
using ClinchApi.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinchApi.Services;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    //Get user by id
    public async Task<UpdateUserModel> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? 
            throw new ArgumentException($"User with ID {userId} not found.");

        return new UpdateUserModel
        {
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Gender = user.Gender,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    // Get all users (optional parameters)
    public List<UpdateUserModel> GetAllUsers()
    {
        var users = _userManager.Users.ToList();

        List<UpdateUserModel> output = [];
        foreach (var user in users)
        {
            var userModel = new UpdateUserModel()
            {
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            output.Add(userModel);
        }
        return output;
    }

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
    public async Task LogOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    // Delete user account
    public async Task<IdentityResult> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError { Description = $"User with ID {userId} not found" });
        }

        // Log out the user
        await _signInManager.SignOutAsync();

        return await _userManager.DeleteAsync(user);
    }


    //ENDPOINTS MANAGED BY IDENTITY
    // - register, login, refresh token
    // - confirm email, resend confirmation email
    // - forgot password, reset password, change password (POST/manage/info)
    // - get logged-in user info (GET/manage/info)
}
