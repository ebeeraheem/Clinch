using ClinchApi.Models.Interfaces;

namespace ClinchApi.Extensions;

public static class AddressValidator
{
    public static T ValidateAddress<T>(T address)
    where T : IAddressBase
    {
        if (address is null)
        {
            throw new ArgumentNullException(nameof(address), "Address cannot be null");
        }

        if (string.IsNullOrWhiteSpace(address.StreetAddress))
        {
            throw new ArgumentException("Street address is required");
        }

        if (string.IsNullOrWhiteSpace(address.City))
        {
            throw new ArgumentException("City is required");
        }

        if (string.IsNullOrWhiteSpace(address.State))
        {
            throw new ArgumentException("State is required");
        }

        if (string.IsNullOrWhiteSpace(address.Country))
        {
            throw new ArgumentException("Country is required");
        }

        return address;
    }
}
