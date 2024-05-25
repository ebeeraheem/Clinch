using ClinchApi.Entities;
using ClinchApi.Entities.DTOs;

namespace ClinchApi.Extensions;

public static class AddressConverter
{
    public static Address ConvertToAddress(this AddressDTO addressDTO)
    {
        var address = new Address()
        {
            Id = 0,
            StreetAddress = addressDTO.StreetAddress,
            City = addressDTO.City,
            State = addressDTO.State,
            PostalCode = addressDTO.PostalCode,
            Country = addressDTO.Country,
            IsBillingAddress = addressDTO.IsBillingAddress,
            IsShippingAddress = addressDTO.IsShippingAddress
        };

        return address;
    }
}
