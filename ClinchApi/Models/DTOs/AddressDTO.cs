using ClinchApi.Entities;
using ClinchApi.Entities.Interfaces;

namespace ClinchApi.Models.DTOs;

public class AddressDTO : IAddressBase
{
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Address type
    public AddressType AddressType { get; set; }
}
