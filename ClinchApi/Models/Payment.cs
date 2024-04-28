namespace ClinchApi.Models;

public class Payment
{
    public int Id { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public string TransactionId { get; set; }
}
public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    PayPal,
    CashOnDelivery
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Cancelled
}
