﻿using ClinchApi.Entities;
using ClinchApi.Extensions;
using ClinchApi.Models.DTOs;
using ClinchApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Get a list of all products with the applied filters
    /// </summary>
    /// <param name="maxPrice"></param>
    /// <param name="minPrice"></param>
    /// <param name="lowStock"></param>
    /// <param name="categoryId"></param>
    /// <param name="createdAfter"></param>
    /// <param name="createdBefore"></param>
    /// <returns>A list of products</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts(
        [FromQuery] decimal? maxPrice,
        [FromQuery] decimal? minPrice,
        [FromQuery] int? lowStock,
        [FromQuery] int? categoryId,
        [FromQuery] DateTime? createdAfter,
        [FromQuery] DateTime? createdBefore)
    {
        var products = await _productService.GetProducts();
        products = ProductFilter.FilterProducts(
            products,
            maxPrice,
            minPrice,
            lowStock,
            categoryId,
            createdAfter,
            createdBefore);

        if (products.Count() == 0)
        {
            return NotFound();
        }

        return Ok(products);
    }

    /// <summary>
    /// Get a product based on its ID
    /// </summary>
    /// <param name="id">ID of the product to return</param>
    /// <returns>A product</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product?>> GetById(int id)
    {
        var product = await _productService.GetProductById(id);

        return product == null ?
            NotFound($"Product with ID {id} not found") :
            Ok(product);
    }

    /// <summary>
    /// Creates a new product with the provided details
    /// </summary>
    /// <param name="newProductDTO">Details of the new product</param>
    /// <returns>A 'Location' response header with the URL of the newly created product</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Store Owner,Store Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> Create([FromBody] ProductDTO newProductDTO)
    {
        try
        {
            var product = await _productService.Create(newProductDTO);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
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
    /// Updates a product
    /// </summary>
    /// <param name="id">ID of the product to be updated</param>
    /// <param name="productUpdateDTO">Information about the product to update</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Store Owner,Store Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDTO productUpdateDTO)
    {
        try
        {
            await _productService.Update(id, productUpdateDTO);
            return NoContent();
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
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
    /// Deletes a product
    /// </summary>
    /// <param name="id">ID of the product to be deleted</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Store Owner,")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _productService.Delete(id);
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
