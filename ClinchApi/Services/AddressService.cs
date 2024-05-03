using ClinchApi.Data;
using ClinchApi.Extensions;
using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class AddressService
{
    private readonly ApplicationDbContext _context;

    public AddressService(ApplicationDbContext context)
    {
        _context = context;
    }

    //Get an address
    public async Task<Address?> GetAddressById(int id)
    {
        return await _context.Addresses.AsNoTracking()
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    //Add an address
    public async Task<Address> Create(AddressDTO addressDTO)
    {
        var validAddressDTO = AddressValidator.ValidateAddress(addressDTO);
        var address = validAddressDTO.ConvertToAddress();

        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        return address;
    }

    //Update an address
    public async Task Update(int id, Address address)
    {
        if (address is null || address.Id != id)
        {
            throw new ArgumentException("Invalid ID or address");
        }

        var validAddress = AddressValidator.ValidateAddress(address);

        var addressToUpdate = await _context.Addresses.FindAsync(id);
        if (addressToUpdate is null)
        {
            throw new InvalidOperationException("Address not found");
        }

        _context.Entry(addressToUpdate).CurrentValues.SetValues(validAddress);

        await _context.SaveChangesAsync();
    }

    //Delete an address
    public async Task Delete(int id)
    {
        var addressToDelete = await _context.Addresses.FindAsync(id);
        if (addressToDelete is null)
        {
            throw new InvalidOperationException("Address not found");
        }

        _context.Addresses.Remove(addressToDelete);
        await _context.SaveChangesAsync();
    }

    

    /* *************************************** */

    //public static AddressDTO ValidateAddress(AddressDTO addressDTO)
    //{
    //    if (addressDTO is null)
    //    {
    //        throw new ArgumentNullException(nameof(addressDTO), "Address cannot be null");
    //    }

    //    if (string.IsNullOrWhiteSpace(addressDTO.StreetAddress))
    //    {
    //        throw new ArgumentException("Street address is required", nameof(addressDTO.StreetAddress));
    //    }

    //    if (string.IsNullOrWhiteSpace(addressDTO.City))
    //    {
    //        throw new ArgumentException("City is required", nameof(addressDTO.City));
    //    }

    //    if (string.IsNullOrWhiteSpace(addressDTO.State))
    //    {
    //        throw new ArgumentException("State is required", nameof(addressDTO.State));
    //    }

    //    if (string.IsNullOrWhiteSpace(addressDTO.Country))
    //    {
    //        throw new ArgumentException("Country is required", nameof(addressDTO.State));
    //    }

    //    return addressDTO;
    //}

}
