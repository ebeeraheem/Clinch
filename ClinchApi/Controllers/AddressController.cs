using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using ClinchApi.Services;
using Microsoft.AspNetCore.Http;
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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Address?>> GetById(int id)
    {
        var address = await _addressService.GetAddressById(id);

        return address == null ? NotFound() : Ok(address);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Address>> Post([FromBody] AddressDTO newAddressDTO)
    {
        try
        {
            var address = await _addressService.Create(newAddressDTO);
            var uri = Url.Action(nameof(GetById), new { id = address.Id });

            return Created(uri, address);
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

    [HttpPut]
    public async Task<ActionResult> Put(int id, [FromBody] Address address)
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
}
