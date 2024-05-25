using ClinchApi.Entities.Interfaces;

namespace ClinchApi.Entities;

public class Address : IAddressBase
{
    public int Id { get; set; }
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Additional properties for identifying address types
    public bool IsBillingAddress { get; set; }
    public bool IsShippingAddress { get; set; }
}
