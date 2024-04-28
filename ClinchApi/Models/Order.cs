namespace ClinchApi.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderNotes { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public int ShippingAddressId { get; set; }
    public virtual Address ShippingAddress { get; set; }
    public int BillingAddressId { get; set; }
    public virtual Address BillingAddress { get; set; }
    public int PaymentId { get; set; }
    public virtual Payment Payment { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}
public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}