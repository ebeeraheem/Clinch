namespace ClinchApi.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string PaymentGateway { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string Currency { get; set; } = "NGN";
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string PaymentReference { get; set; } = string.Empty;
}
public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    PayPal,
    GiftCard,
    Wallet,
    BankTransfer,
    CashOnDelivery
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Cancelled,
    Refunded
}
