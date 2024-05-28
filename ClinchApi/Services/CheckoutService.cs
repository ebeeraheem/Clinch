using ClinchApi.Data;
using ClinchApi.Entities;
using ClinchApi.Models;
using ClinchApi.Models.Results;
using Microsoft.AspNetCore.Identity;

namespace ClinchApi.Services;

public class CheckoutService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CheckoutService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<CheckoutResult> ProcessCheckoutAsync(CheckoutModel checkoutModel)
    {
        var user = await _userManager.FindByEmailAsync(checkoutModel.EmailAddress);

        if (user is null)
        {
            return new CheckoutResult
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Calculate total amount
        var totalAmount = checkoutModel.Items.Sum(item => item.UnitPrice * item.Quantity);

        // Process payment
    }
}
