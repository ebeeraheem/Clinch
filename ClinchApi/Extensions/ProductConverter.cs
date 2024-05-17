using ClinchApi.Data;
using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
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

    public static async Task<Product> UpdateToProduct(this ProductUpdateDTO productUpdateDTO, ApplicationDbContext context, Product productToUpdate)
    {
        var product = new Product()
        {
            Id = productUpdateDTO.Id,
            Name = productUpdateDTO.Name,
            Description = productUpdateDTO.Description,
            Price = productUpdateDTO.Price,
            Quantity = productUpdateDTO.Quantity,
            CategoryId = productUpdateDTO.CategoryId!,
            //Categories, if any, will be added below,
            ImageUrl = productUpdateDTO.ImageUrl,
            CreatedAt = productToUpdate.CreatedAt, //This remains the same
            LastUpdatedAt = DateTime.UtcNow
        };

        var categoriesToRemove = productToUpdate.Categories
            .Select(c => c.Id).Except(product.CategoryId!).ToList();
        var categoriesToAdd = product.CategoryId!.Except(productToUpdate.Categories.Select(c => c.Id)).ToList();

        //Remove categories
        foreach (var categoryIdToRemove in categoriesToRemove)
        {
            var categoryToRemove = productToUpdate.Categories.FirstOrDefault(c => c.Id == categoryIdToRemove);
            if (categoryToRemove is not null)
            {
                productToUpdate.Categories.Remove(categoryToRemove);
            }
        }

        //Add new categories
        foreach (var categoryIdToAdd in categoriesToAdd)
        {
            var categoryToAdd = context.Categories.Find(categoryIdToAdd);
            if (categoryToAdd is not null)
            {
                productToUpdate.Categories.Add(categoryToAdd);
            }
        }
        await context.SaveChangesAsync();

        return product;
    }
}
