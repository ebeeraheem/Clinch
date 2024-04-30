using ClinchApi.Data;
using ClinchApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class AddressService
{
    private readonly EcommerceDbContext _context;

    public AddressService(EcommerceDbContext context)
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
    public async Task<Address> Create(Address address)
    {
        var validAddress = ValidateAddress(address);

        _context.Addresses.Add(validAddress);
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

        var validAddress = ValidateAddress(address);

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

    public static Address ValidateAddress(Address address)
    {
        if (address is null)
        {
            throw new ArgumentNullException(nameof(address), "Address cannot be null");
        }

        if (string.IsNullOrWhiteSpace(address.StreetAddress))
        {
            throw new ArgumentException("Street address is required", nameof(address.StreetAddress));
        }

        if (string.IsNullOrWhiteSpace(address.City))
        {
            throw new ArgumentException("City is required", nameof(address.City));
        }

        if (string.IsNullOrWhiteSpace(address.State))
        {
            throw new ArgumentException("State is required", nameof(address.State));
        }

        return address;
    }
}
