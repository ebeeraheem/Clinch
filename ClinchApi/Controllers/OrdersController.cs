using ClinchApi.Entities;
using ClinchApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Gets a list of all orders with optional parameters
    /// </summary>
    /// <param name="orderStatus">Optional status of the order to return</param>
    /// <returns>A list of orders</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Order>>> GetOrders([FromQuery] OrderStatus? orderStatus)
    {
        var orders = await _orderService.GetAllOrders();

        if (orderStatus.HasValue)
        {
            orders = orders.Where(o => o.OrderStatus == orderStatus).ToList();
        }

        return orders.Count == 0 ? NotFound() : Ok(orders);
    }

    /// <summary>
    /// Get a users orders
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>A list of orders</returns>
    [HttpGet("users/{userId}/orders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<Order>> GetUserOrders(string userId)
    {
        return _orderService.GetUserOrders(userId);
    }

    /// <summary>
    /// Get an order
    /// </summary>
    /// <param name="orderId">Id of the order to return</param>
    /// <returns>An order</returns>
    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Order>> GetOrderById(int orderId)
    {
        try
        {
            return await _orderService.GetOrder(orderId);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occured");
        }
    }

    /// <summary>
    /// Get orders that have a particular product
    /// </summary>
    /// <param name="productId">Id of the product</param>
    /// <returns>A list of products</returns>
    [HttpGet("products/{productId}/orders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Order>>> GetOrderByProductId(int productId)
    {
        try
        {
            return await _orderService.GetOrdersByProductId(productId);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occured");
        }
    }

    /// <summary>
    /// Update the status of an order
    /// </summary>
    /// <param name="orderId">The id of the order to update</param>
    /// <param name="newStatus">The new status of the order</param>
    /// <returns>No content</returns>
    [HttpPatch("{orderId}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        try
        {
            await _orderService.UpdateOrderStatus(orderId, newStatus);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occured");
        }
    }
}
