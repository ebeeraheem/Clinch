using ClinchApi.Data;
using ClinchApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class ProductService
{
    private readonly EcommerceDbContext _context;

    public ProductService(EcommerceDbContext context)
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
    public async Task<Product> Create(Product product)
    {
        var validProduct = ValidateProduct(product);

        _context.Products.Add(validProduct);
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

        var validProduct = ValidateProduct(newProduct);

        var productToUpdate = await _context.Products.FindAsync(id);
        if (productToUpdate is null)
        {
            throw new InvalidOperationException("Product not found");
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
            throw new InvalidOperationException("Product not found");
        }

        _context.Products.Remove(productToDelete);
        await _context.SaveChangesAsync();
    }

    /* *************************************** */

    public static Product ValidateProduct(Product product)
    {
        var context = new EcommerceDbContext();
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(product.Name));
        }

        if (context.Products.Any(p => p.Name == product.Name))
        {
            throw new InvalidOperationException("Product with the same name already exists");
        }

        if (product.Price <= 0)
        {
            throw new ArgumentException("Price must be greater than zero", nameof(product.Price));
        }

        if (product.Quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero", nameof(product.Quantity));
        }

        ////Check whether the provided image Uri is valid
        //if (!Uri.TryCreate(product.ImageUrl.ToString(), UriKind.Absolute, out Uri imageUri))
        //{
        //    throw new ArgumentException("Invalid image URL format", nameof(product.ImageUrl));
        //}

        return product;
    }
}
