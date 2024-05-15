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
}
