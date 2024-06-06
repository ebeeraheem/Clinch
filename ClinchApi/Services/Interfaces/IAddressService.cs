using ClinchApi.Entities;
using ClinchApi.Models.DTOs;

namespace ClinchApi.Services.Interfaces
{
    public interface IAddressService
    {
        Task<Address> Create(int userId, AddressDTO addressDTO);
        Task Delete(int userId, int addressId);
        Address? GetAddressById(int userId, int addressId);
        List<Address> GetAddresses(int userId);
        Task Update(int userId, int addressId, Address address);
    }
}