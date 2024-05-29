using ClinchApi.Entities;

namespace ClinchApi.Models;

public class CheckoutModel
{
    //public required string FirstName { get; set; }
    //public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
    //public required string PhoneNumber { get; set; }
    public required string PaymentToken { get; set; }
    public string? Notes { get; set; }

    //public int BillingAddressId { get; set; }
    public string? BillingAddress { get; set; }

    //public int ShippingAddressId { get; set; }
    public string? ShippingAddress { get; set; }

    public required List<OrderItem> Items { get; set; }
}
