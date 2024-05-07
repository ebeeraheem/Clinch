using ClinchApi.Data;
using ClinchApi.Extensions;
using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using ClinchApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    //Get all products
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Product>> Post(ProductDTO newProductDTO)
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

    //Update an existing product

    //Delete a product

}
