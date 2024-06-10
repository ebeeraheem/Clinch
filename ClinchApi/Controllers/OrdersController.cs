﻿using ClinchApi.Entities;
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Order>>> GetOrders([FromQuery] OrderStatus? orderStatus)
    {
        var orders = await _orderService.GetAllOrders();

        if (orderStatus.HasValue)
        {
            orders = orders.Where(o => o.OrderStatus == orderStatus).ToList();
        }

        return orders.Count == 0 ? NotFound() : Ok(orders);
    }

    [HttpGet("by_userid{userId}")]
    public ActionResult<List<Order>> GetUserOrders(string userId)
    {
        return _orderService.GetUserOrders(userId);
    }

    [HttpGet("by_orderid/{orderId}")]
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

    [HttpGet("by_productid/{productId}")]
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

    [HttpPatch("{orderId}/status")]
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