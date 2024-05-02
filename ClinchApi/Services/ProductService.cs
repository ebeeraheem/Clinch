using ClinchApi.Data;
using ClinchApi.Models;
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
    public async Task<Product> Create(Product product)
    {
        var validProduct = await ValidateProduct(product, _context);

        await _context.Products.AddAsync(validProduct);
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

        var validProduct = await ValidateProduct(newProduct, _context, true, id);

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

    public static async Task<Product> ValidateProduct(Product product, ApplicationDbContext context, bool isUpdate = false, int id = 0)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null");
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(product.Name));
        }

        //Check whether a product with the same name exist
        //Or a product with the same name that is not the one being updated
        if (await context.Products.AnyAsync(p => p.Name == product.Name && (isUpdate ? p.Id != id : true)))
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
