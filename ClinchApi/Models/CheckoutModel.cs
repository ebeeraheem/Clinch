using ClinchApi.Entities;

namespace ClinchApi.Models;

public class CheckoutModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public int PaymentId { get; set; }
    public virtual Payment? Payment { get; set; }

    public int BillingAddressId { get; set; }
    public virtual Address? BillingAddress { get; set; }

    public int ShippingAddressId { get; set; }
    public virtual Address? ShippingAddress { get; set; }
}
