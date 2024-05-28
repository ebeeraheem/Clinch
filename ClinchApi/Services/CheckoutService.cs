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
    private readonly PaymentService _paymentService;

    public CheckoutService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, PaymentService paymentService)
    {
        _context = context;
        _userManager = userManager;
        _paymentService = paymentService;
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
        var totalAmount = checkoutModel.Items.Sum(item => item.Price * item.Quantity);

        // Process payment
        var paymentResult = await _paymentService.ProcessPaymentAsync(checkoutModel.PaymentToken, totalAmount);

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
            UserId = user.Id.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            OrderDate = DateTime.UtcNow,
            OrderNotes = checkoutModel.Notes,
            OrderStatus = OrderStatus.Pending,
            TotalAmount = totalAmount,
            PaymentDetails = paymentResult.TransactionId,
            OrderItems = checkoutModel.Items.Select(item => new OrderItem()
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,
                //OrderId = item.OrderId,
            }).ToList(),
            BillingAddress = checkoutModel.BillingAddress,
            ShippingAddress = checkoutModel.ShippingAddress
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
