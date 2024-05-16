using ClinchApi.Models;
using ClinchApi.Services;
using Microsoft.AspNetCore.Http;
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

    [HttpGet("{userId}/cart")]
    public async Task<ActionResult<List<ShoppingCartItem>>> GetCart(int userId)
    {
        return await _cartService.GetCart(userId);
    }

    [HttpPost("{userId}/cart/add/{productId}")]
    public async Task<ActionResult<ShoppingCart>> AddToCart(int userId, int productId)
    {
        try
        {
            return await _cartService.AddToCart(userId, productId);
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

    [HttpPost("{userId}/cart/increase/{productId}")]
    public async Task<ActionResult<ShoppingCart>> IncreaseQuantity(int userId, int productId)
    {
        try
        {
            return await _cartService.IncreaseQuantity(userId, productId);
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

    [HttpPost("{userId}/cart/decrease/{productId}")]
    public async Task<ActionResult<ShoppingCart>> DecreaseQuantity(int userId, int productId)
    {
        try
        {
            return await _cartService.DecreaseQuantity(userId, productId);
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

    [HttpDelete("{userId}/cart/remove/{productId}")]
    public async Task<ActionResult<ShoppingCart>> RemoveFromCart(int userId, int productId)
    {
        try
        {
            var updatedCart = await _cartService
                .RemoveFromCart(userId, productId);

            return updatedCart;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    [HttpDelete("{userId}/cart/clear")]
    public async Task<ActionResult<ShoppingCart>> ClearCart(int userId)
    {
        try
        {
            return await _cartService.ClearCart(userId);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }
}
