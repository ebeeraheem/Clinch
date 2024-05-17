using ClinchApi.Models;
using ClinchApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartsController : ControllerBase
{
    private readonly ShoppingCartService _cartService;

    public ShoppingCartsController(ShoppingCartService cartService)
    {
        _cartService = cartService;
    }

    /// <summary>
    /// Get all the items in a user's cart
    /// </summary>
    /// <param name="userId">ID of the user whose cart is to be retrieved</param>
    /// <returns>A list of shopping cart items</returns>
    [HttpGet("{userId}/cart")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ShoppingCartItem>>> GetCart(int userId)
    {
        var cart = await _cartService.GetCart(userId);
        return Ok(cart);
    }

    /// <summary>
    /// Adds a new product to the user's shopping cart
    /// </summary>
    /// <param name="userId">ID of the user whose cart the product is to be added to</param>
    /// <param name="productId">ID of the product to add to the cart</param>
    /// <returns>The user's shopping cart</returns>
    [HttpPost("{userId}/cart/add/{productId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ShoppingCart>> AddToCart(int userId, int productId)
    {
        try
        {
            var updatedCart = await _cartService.AddToCart(userId, productId);
            return CreatedAtAction(nameof(GetCart), new { userId }, updatedCart);
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
    /// Increases the quantity of an item in the user's cart
    /// </summary>
    /// <param name="userId">ID of the user whose item is to be increased</param>
    /// <param name="productId">ID of the product whose quantity is to be increased</param>
    /// <returns>The user's shopping cart</returns>
    [HttpPost("{userId}/cart/increase/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ShoppingCart>> IncreaseQuantity(int userId, int productId)
    {
        try
        {
            var updatedCart = await _cartService.IncreaseQuantity(userId, productId);
            return Ok(updatedCart);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Decreases the quantity of an item in the user's cart
    /// </summary>
    /// <param name="userId">ID of the user whose item is to be decreased</param>
    /// <param name="productId">ID of the product whose quantity is to be decreased</param>
    /// <returns>The user's shopping cart</return
    [HttpPost("{userId}/cart/decrease/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ShoppingCart>> DecreaseQuantity(int userId, int productId)
    {
        try
        {
            var updatedCart = await _cartService.DecreaseQuantity(userId, productId);
            return Ok(updatedCart);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Removes an item from the user's cart
    /// </summary>
    /// <param name="userId">ID of the user whose cart the item will be removed from</param>
    /// <param name="productId">ID of the product to be removed</param>
    /// <returns>The user's shopping cart</returns>
    [HttpDelete("{userId}/cart/remove/{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ShoppingCart>> RemoveFromCart(int userId, int productId)
    {
        try
        {
            await _cartService.RemoveFromCart(userId, productId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Removes all the items in a users shopping cart
    /// </summary>
    /// <param name="userId">ID of the user whose cart is to be cleared</param>
    /// <returns>The user's empty cart</returns>
    [HttpDelete("{userId}/cart/clear")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ShoppingCart>> ClearCart(int userId)
    {
        try
        {
            await _cartService.ClearCart(userId);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }
}
