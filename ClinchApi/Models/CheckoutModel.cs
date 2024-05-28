using ClinchApi.Entities;

namespace ClinchApi.Models;

public class CheckoutModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PaymentToken { get; set; }

    public int PaymentId { get; set; }
    public virtual Payment? Payment { get; set; }

    public int BillingAddressId { get; set; }
    public virtual Address? BillingAddress { get; set; }

    public int ShippingAddressId { get; set; }
    public virtual Address? ShippingAddress { get; set; }
}
