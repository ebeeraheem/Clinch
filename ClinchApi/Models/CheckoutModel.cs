using ClinchApi.Entities;

namespace ClinchApi.Models;

public class CheckoutModel
{
    // In order to checkout, a user must have provided the following info
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;

    public int PaymentId { get; set; }
    public virtual Payment? Payment { get; set; }

    public int BillingAddressId { get; set; }
    public virtual Address? BillingAddress { get; set; }

    public int ShippingAddressId { get; set; }
    public virtual Address? ShippingAddress { get; set; }
}
