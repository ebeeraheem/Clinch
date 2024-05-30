using Microsoft.AspNetCore.Identity;

namespace ClinchApi.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public Guid UserGuid { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public Gender? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; } // Ensure a valid date of birth

    // User addresses
    public List<Address> Addresses { get; set; } = [];

    public virtual ICollection<Order>? Orders { get; set; }
}

public enum Gender
{
    Male,
    Female,
    Other
}