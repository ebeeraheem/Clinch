using ClinchApi.Models;
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
    /// Get an address by its ID
    /// </summary>
    /// <param name="id">ID of the address to return</param>
    /// <returns>An address</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Address?>> GetById(int id)
    {
        var address = await _addressService.GetAddressById(id);

        return address == null ?
            NotFound($"Address with ID {id} not found") :
            Ok(address);
    }

    /// <summary>
    /// Creates a new address
    /// </summary>
    /// <param name="newAddressDTO">The information of the address to be created</param>
    /// <returns>The newly created address</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Address>> Create([FromBody] AddressDTO newAddressDTO)
    {
        try
        {
            var address = await _addressService.Create(newAddressDTO);

            return CreatedAtAction(nameof(GetById), new { id = address.Id }, address);
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
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update(int id, [FromBody] Address address)
    {
        try
        {
            await _addressService.Update(id, address);
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
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _addressService.Delete(id);
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
