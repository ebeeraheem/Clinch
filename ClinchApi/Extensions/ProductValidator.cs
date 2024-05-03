using ClinchApi.Data;
using ClinchApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Extensions;

public class ProductValidator
{
    public static async Task<T> ValidateProduct<T>(T product, 
        ApplicationDbContext context,
        bool isUpdate = false,
        int id = 0) 
        where T : IProductBase
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

        //Check whether the provided image Uri is valid
        if (!Uri.TryCreate(product.ImageUrl.ToString(), UriKind.Absolute, out Uri imageUri))
        {
            throw new ArgumentException("Invalid image URL format", nameof(product.ImageUrl));
        }

        return product;
    }
}
