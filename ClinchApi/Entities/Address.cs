using ClinchApi.Entities.Interfaces;
using System.Text;

namespace ClinchApi.Entities;

public class Address : IAddressBase
{
    public int Id { get; set; }
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Address type
    public AddressType AddressType { get; set; }

    // Get full address
    public string GetFullAddress()
    {
        string address = $"{StreetAddress}, {City}, {State}, {Country}";

        if (!string.IsNullOrWhiteSpace(PostalCode))
        {
            address += $". Postal Code: {PostalCode}";
        }

        return address;
    }
}

// Enum to represent address types
public enum AddressType
{
    UserAddress,
    BillingAddress,
    ShippingAddress
}
