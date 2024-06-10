using ClinchApi.Entities;

namespace ClinchApi.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrders();
        Task<Order> GetOrder(int orderId);
        Task<List<Order>> GetOrdersByProductId(int productId);
        List<Order> GetUserOrders(string userId);
        Task UpdateOrderStatus(int orderId, OrderStatus newStatus);
    }
}