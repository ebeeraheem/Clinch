using ClinchApi.Data;
using ClinchApi.Extensions;
using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }
    //Get all products
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context.Products.AsNoTracking().ToListAsync();
    }

    //Get product by id
    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products.AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id  == id);
    }

    //Create a product
    public async Task<Product> Create(ProductDTO newProductDTO)
    {
        var validProductDTO = await ProductValidator
            .ValidateProduct(newProductDTO, _context);

        var product = validProductDTO.ConvertToProduct();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    //Update a product
    public async Task Update(int id, Product newProduct)
    {
        if (newProduct is null || newProduct.Id != id)
        {
            throw new ArgumentException("Invalid ID or product");
        }

        var validProduct = await ProductValidator
            .ValidateProduct(newProduct, _context, true, id);

        var productToUpdate = await _context.Products.FindAsync(id);
        if (productToUpdate is null)
        {
            throw new InvalidOperationException($"Product with id {id} not found");
        }

        _context.Entry(productToUpdate).CurrentValues.SetValues(validProduct);

        await _context.SaveChangesAsync();
    }

    //Delete a product
    public async Task Delete(int id)
    {
        var productToDelete = await _context.Products.FindAsync(id);

        if (productToDelete is null)
        {
            throw new InvalidOperationException($"Product with id {id} not found");
        }

        _context.Products.Remove(productToDelete);
        await _context.SaveChangesAsync();
    }
}
