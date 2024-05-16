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

    [HttpGet]
    public async Task<ActionResult<List<ShoppingCartItem>>> GetCart(int userId)
    {
        return await _cartService.GetCart(userId);
    }

    [HttpPost("{userId}/AddToCart/{productId}")]
    public async Task<ActionResult<ShoppingCart>> AddToCart(int userId, int productId)
    {
        try
        {
            var cart = await _cartService.AddToCart(userId, productId);
            return cart;
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

    [HttpPost("{userId}/IncreaseQuantity/{productId}")]
    public async Task<ActionResult<ShoppingCart>> IncreaseQuantity(int userId, int productId)
    {
        try
        {
            var increaseResponse = await _cartService
                .IncreaseQuantity(userId, productId);

            return increaseResponse;
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

    [HttpPost("{userId}/DecreaseQuantity/{productId}")]
    public async Task<ActionResult<ShoppingCart>> DecreaseQuantity(int userId, int productId)
    {
        try
        {
            var decreaseResponse = await _cartService
                .DecreaseQuantity(userId, productId);

            return decreaseResponse;
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

    [HttpDelete("{userId}/RemoveFromCart/{productId}")]
    public async Task<ActionResult<ShoppingCart>> RemoveFromCart(int userId, int productId)
    {
        try
        {
            var removeResponse = await _cartService
                .RemoveFromCart(userId, productId);

            return removeResponse;
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
