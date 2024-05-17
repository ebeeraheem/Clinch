using System.ComponentModel.DataAnnotations;

namespace ClinchApi.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }

    [Required]
    public virtual Order Order { get; set; }
    public int ProductId { get; set; }

    [Required]
    public virtual Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public OrderItem(int orderId, Order order, int productId, Product product, int quantity, decimal price)
    {
        OrderId = orderId;
        Order = order;
        ProductId = productId;
        Product = product;
        Quantity = quantity;
        Price = price;
    }
}
