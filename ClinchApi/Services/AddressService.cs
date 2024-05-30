using ClinchApi.Data;
using ClinchApi.Extensions;
using ClinchApi.Entities;
using Microsoft.EntityFrameworkCore;
using ClinchApi.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Services;

public class AddressService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AddressService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Get a user's addresses
    public List<Address> GetAddresses(int userId)
    {
        // Get the user
        var user = _userManager.Users
            .SingleOrDefault(u => u.Id == userId) ??
            throw new InvalidOperationException($"User with ID {userId} not found");

        return user.Addresses;
    }

    //Get an address
    public Address? GetAddressById(int userId, int addressId)
    {
        // Get the user
        var user = _userManager.Users
            .SingleOrDefault(u => u.Id == userId) ??
            throw new InvalidOperationException($"User with ID {userId} not found");

        // Get the user's address
        var address = user.Addresses
            .SingleOrDefault(a => a.Id == addressId) ??
            throw new InvalidOperationException("Address not found");

        return address;
    }

    //Add an address
    public async Task<Address> Create(int userId, AddressDTO addressDTO)
    {
        // Get the user
        var user = _userManager.Users
            .SingleOrDefault(u => u.Id == userId) ?? 
            throw new InvalidOperationException($"User with ID {userId} not found");

        var validAddressDTO = AddressValidator.ValidateAddress(addressDTO);
        var address = validAddressDTO.ConvertToAddress();

        user.Addresses.Add(address);
        await _context.SaveChangesAsync();

        return address;
    }

    //Update an address
    public async Task Update(int userId, int addressId, Address address)
    {
        if (address is null || address.Id != addressId)
        {
            throw new ArgumentException("Invalid ID or address");
        }

        // Get the user
        var user = _userManager.Users
            .SingleOrDefault(u => u.Id == userId) ??
            throw new InvalidOperationException($"User with ID {userId} not found");

        // Get the user's address
        var addressToUpdate = user.Addresses
            .SingleOrDefault(a => a.Id == addressId) ?? 
            throw new InvalidOperationException("Address not found");

        // Validate the new address
        var validAddress = AddressValidator.ValidateAddress(address);

        // Update the address
        addressToUpdate = validAddress;

        await _context.SaveChangesAsync();
    }

    //Delete an address
    public async Task Delete(int userId, int addressId)
    {
        // Get the user
        var user = _userManager.Users
            .SingleOrDefault(u => u.Id == userId) ??
            throw new InvalidOperationException($"User with ID {userId} not found");

        // Get the user's address
        var addressToDelete = user.Addresses
            .SingleOrDefault(a => a.Id == addressId) ??
            throw new InvalidOperationException("Address not found");

        // Remove the address
        user.Addresses.Remove(addressToDelete);
        await _context.SaveChangesAsync();
    }
}
