using ClinchApi.Data;
using ClinchApi.Entities;
using ClinchApi.Extensions;
using ClinchApi.Models.DTOs;
using ClinchApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }
    //Get all products
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context.Products.AsNoTracking()
            .Include(p => p.Categories)
            .ToListAsync();
    }

    //Get product by id
    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products.AsNoTracking()
            .Include(p => p.Categories)
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    //Create a product
    public async Task<Product> Create(ProductDTO newProductDTO)
    {
        var validProductDTO = await ProductValidator
            .ValidateProduct(newProductDTO, _context);

        var product = await validProductDTO.ConvertToProduct(_context);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    //Update a product
    public async Task Update(int id, ProductUpdateDTO productUpdateDTO)
    {
        if (productUpdateDTO is null || productUpdateDTO.Id != id)
        {
            throw new ArgumentException("Invalid ID or product");
        }

        //Validate the product update DTO
        var validProductDTO = await ProductValidator
            .ValidateProduct(productUpdateDTO, _context, true, id);

        //Get the product to update by its ID
        var productToUpdate = await _context.Products
            .Include(p => p.Categories)
            .SingleOrDefaultAsync(p => p.Id == id) ?? 
            throw new InvalidOperationException($"Product with id {id} not found");

        //Convert the product update DTO to Product
        var product = await validProductDTO
            .UpdateToProduct(_context, productToUpdate);

        _context.Entry(productToUpdate).CurrentValues.SetValues(product);
        await _context.SaveChangesAsync();
    }

    //Delete a product
    public async Task Delete(int id)
    {
        var productToDelete = await _context.Products.FindAsync(id) ?? 
            throw new InvalidOperationException($"Product with id {id} not found");

        _context.Products.Remove(productToDelete);
        await _context.SaveChangesAsync();
    }
}
