using ClinchApi.Data;
using ClinchApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class OrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }
    // Get all orders (by date, by status)
    public async Task<List<Order>> GetAllOrders()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .ToListAsync();
    }

    // Get a user's orders
    public List<Order> GetUserOrders(string userId)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId.ToString() == userId)
            .ToList();
    }

    // Get an order
    public async Task<Order> GetOrder(int orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId) ??
            throw new InvalidOperationException($"Order with id {orderId} not found");
    }

    // Get orders by product
    public async Task<List<Order>> GetOrdersByProductId(int productId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.OrderItems != null && 
                o.OrderItems.Any(oi => oi.ProductId == productId))
            .ToListAsync() ??
            throw new InvalidOperationException($"Order with productId {productId} not found");
    }

    // Update order status
    public async Task UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        var order = await _context.Orders.FindAsync(orderId) ??
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");

        order.OrderStatus = newStatus;

        await _context.SaveChangesAsync();
    }
}
