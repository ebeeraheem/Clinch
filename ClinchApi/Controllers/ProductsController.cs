using ClinchApi.Data;
using ClinchApi.Extensions;
using ClinchApi.Models;
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

    //Get a product by ID

    //Add a new product

    //Update an existing product

    //Delete a product

    //------------------------------------------//
    //Get products where max-price <= n
    //Get products where min-price <= n
    //Get products where price between min - max
    //Get products that are low on stock
    //Get all products from a category
    //Get products created after {date}
    //Get products created before {date}

}
