using ClinchApi.Models;
using ClinchApi.Models.DTOs;

namespace ClinchApi.Services.Interfaces;

public interface IAddressService
{
    Task<Address?> GetAddressById(int id);
    Task<AddressDTO> Create(AddressDTO addressDTO);
    Task Update(int id, Address address);
    Task Delete(int id);
}
