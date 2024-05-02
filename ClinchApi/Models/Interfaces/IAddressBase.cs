namespace ClinchApi.Models.Interfaces;

public interface IAddressBase
{
    string StreetAddress { get; set; }
    string City { get; set; }
    string State { get; set; }
    string Country { get; set; }
}
