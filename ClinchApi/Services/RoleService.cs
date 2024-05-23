using ClinchApi.Models;
using Microsoft.AspNetCore.Identity;

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
    

}
