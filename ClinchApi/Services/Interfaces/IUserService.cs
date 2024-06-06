using ClinchApi.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinchApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> DeleteUserAsync(string userId);
        List<UpdateUserModel> GetAllUsers();
        Task<UpdateUserModel> GetUserByIdAsync(string userId);
        Task LogOutAsync();
        Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserModel model);
    }
}