namespace ClinchApi.Models;

public class CheckoutModel
{
    public required string PaymentToken { get; set; }
    public string? Notes { get; set; }
}
