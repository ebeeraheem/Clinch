namespace ClinchApi.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserId {  get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public string PaymentDetails { get; set; } = string.Empty;
    public string? OrderNotes { get; set; }
    public OrderStatus OrderStatus { get; set; }
    //public int? BillingAddressId { get; set; }
    public string? BillingAddress { get; set; }
    //public int ShippingAddressId { get; set; }
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