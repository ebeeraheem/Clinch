namespace ClinchApi.Entities;

public class Order
{
    public int Id { get; set; }
    public int UserId {  get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentDetails { get; set; } = string.Empty;
    public string? OrderNotes { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string? BillingAddress { get; set; }
    public string? ShippingAddress { get; set; }
    public virtual List<OrderItem>? OrderItems { get; set; }
}
public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}