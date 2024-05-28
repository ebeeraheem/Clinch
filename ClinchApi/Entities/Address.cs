﻿using ClinchApi.Entities.Interfaces;
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

    // Additional properties for identifying address types
    public bool IsBillingAddress { get; set; }
    public bool IsShippingAddress { get; set; }

    //Get full address
    public string GetFullAddress()
    {
        var fullAddress = new StringBuilder();
        if (!string.IsNullOrEmpty(StreetAddress))
        {
            fullAddress.AppendLine(StreetAddress);
        }
        if (!string.IsNullOrEmpty(City))
        {
            fullAddress.Append(City);
            if (!string.IsNullOrEmpty(State))
            {
                fullAddress.Append(", ");
            }
        }
        if (!string.IsNullOrEmpty(State))
        {
            fullAddress.Append(State);
        }
        if (!string.IsNullOrEmpty(PostalCode))
        {
            fullAddress.Append(" ");
            fullAddress.Append(PostalCode);
        }
        if (!string.IsNullOrEmpty(Country))
        {
            fullAddress.AppendLine();
            fullAddress.Append(Country);
        }

        return fullAddress.ToString().Trim();
    }

}
