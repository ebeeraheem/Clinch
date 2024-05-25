using Microsoft.AspNetCore.Identity;

namespace ClinchApi.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public Guid UserGuid { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public Gender? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }

    // Navigation properties for addresses
    public int? UserAddressId { get; set; }
    public virtual Address? UserAddress { get; set; }

    public int? BillingAddressId { get; set; }
    public virtual Address? BillingAddress { get; set; }

    public int? ShippingAddressId { get; set; }
    public virtual Address? ShippingAddress { get; set; }

    public int? PaymentId { get; set; }
    public virtual Payment? Payment { get; set; }

    public virtual ICollection<Order>? Orders { get; set; }
}

public enum Gender
{
    Male,
    Female,
    Other
}