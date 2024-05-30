using ClinchApi.Data;
using ClinchApi.Entities;
using ClinchApi.Models;
using ClinchApi.Models.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class CheckoutService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly PaymentService _paymentService;

    public CheckoutService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, PaymentService paymentService)
    {
        _context = context;
        _userManager = userManager;
        _paymentService = paymentService;
    }

    public async Task<CheckoutResult> ProcessCheckoutAsync(CheckoutModel checkoutModel)
    {
        // Get the user
        var user = await _userManager.FindByIdAsync(checkoutModel.UserId);
        if (user is null)
        {
            return new CheckoutResult
            {
                Success = false,
                Message = $"User with ID {checkoutModel.UserId} not found."
            };
        }
        // Get the user's cart
        var cart = await _context.ShoppingCarts
            .Include(c => c.ShoppingCartItems)
            .SingleOrDefaultAsync(c => c.UserId.ToString() == checkoutModel.UserId);

        if (cart is null)
        {
            return new CheckoutResult
            {
                Success = false,
                Message = "Cart not found"
            };
        }

        // Calculate the total amount of the order
        var totalAmount = cart.ShoppingCartItems.Sum(item => item.UnitPrice * item.Quantity);

        // Process payment
        var paymentResult = await _paymentService
            .ProcessPaymentAsync(checkoutModel.PaymentToken, totalAmount);

        if (!paymentResult.Success)
        {
            return new CheckoutResult
            {
                Success = false,
                Message = paymentResult.Message
            };
        }

        // Process order logic
        var order = new Order()
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            OrderDate = DateTime.UtcNow,
            OrderNotes = checkoutModel.Notes,
            OrderStatus = OrderStatus.Pending,
            TotalAmount = totalAmount,
            PaymentDetails = paymentResult.TransactionId,
            OrderItems = cart.ShoppingCartItems.Select(item => new OrderItem()
            {
                ProductId = item.ProductId,
                Price = item.UnitPrice,
                Quantity = item.Quantity
            }).ToList(),
            BillingAddress = user.BillingAddress!.GetFullAddress(),
            ShippingAddress = user.ShippingAddress!.GetFullAddress()
        };

        // Save order to database
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return new CheckoutResult
        {
            Success = true,
            Message = "Order processed successfully."
        };
    }
}
