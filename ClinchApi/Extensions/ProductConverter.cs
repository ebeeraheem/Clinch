using ClinchApi.Data;
using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using System.Runtime.CompilerServices;

namespace ClinchApi.Extensions;

public static class ProductConverter
{
    public static async Task<Product> ConvertToProduct(this ProductDTO productDTO, ApplicationDbContext context)
    {
        var product = new Product()
        {
            Id = 0,
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            Quantity = productDTO.Quantity,
            CategoryId = productDTO.CategoryId,
            //Categories, if any, are added below,
            ImageUrl = productDTO.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        // Fetch categories from the database based on CategoryIds
        if (productDTO.CategoryId is not null && productDTO.CategoryId.Any())
        {
            product.Categories = new List<Category>();
            foreach (var id in productDTO.CategoryId)
            {
                var category = await context.Categories.FindAsync(id);
                if (category is null)
                {
                    throw new ArgumentException($"Category with id {id} does not exist");
                }
                // Add the category to the product's Categories collection
                product.Categories.Add(category);
            }
        }

        return product;
    }
}
