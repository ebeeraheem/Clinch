using ClinchApi.Entities;
using ClinchApi.Models.DTOs;
using ClinchApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly AddressService _addressService;

    public AddressController(AddressService addressService)
    {
        _addressService = addressService;
    }

    /// <summary>
    /// Get a list of a user's addresses
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <returns>A list of the user's addresses</returns>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<List<Address>> GetAddresses(int userId)
    {
        try
        {
            return _addressService.GetAddresses(userId);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Get an address by its ID
    /// </summary>
    /// <param name="id">ID of the address to return</param>
    /// <returns>An address</returns>
    [HttpGet("{userId}/{addressId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Address?> GetAddress(int userId, int addressId)
    {
        try
        {
            var address = _addressService.GetAddressById(userId, addressId);

            return address is null ?
            NotFound($"Address with ID {addressId} not found") :
            Ok(address);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Creates a new address
    /// </summary>
    /// <param name="newAddressDTO">The information of the address to be created</param>
    /// <returns>The newly created address</returns>
    [HttpPost("{userId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Address>> Create(int userId, [FromBody] AddressDTO newAddressDTO)
    {
        try
        {
            var address = await _addressService.Create(userId, newAddressDTO);

            return CreatedAtAction(nameof(GetAddress), new { userId, addressId = address.Id }, address);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Updates an address
    /// </summary>
    /// <param name="id">ID of the address to be updated</param>
    /// <param name="address">Information to update the address</param>
    /// <returns>No content</returns>
    [HttpPut("{userId}/{addressId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update(int userId, int addressId, [FromBody] Address address)
    {
        try
        {
            await _addressService.Update(userId, addressId, address);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Delete an address
    /// </summary>
    /// <param name="id">ID of the address to be deleted</param>
    /// <returns>No content</returns>
    [HttpDelete("{userId}/{addressId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int userId, int addressId)
    {
        try
        {
            await _addressService.Delete(userId, addressId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred");
        }
    }
}
