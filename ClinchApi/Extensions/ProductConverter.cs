using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using System.Runtime.CompilerServices;

namespace ClinchApi.Extensions;

public static class ProductConverter
{
    public static Product ConvertToProduct(this ProductDTO productDTO)
    {
        var product = new Product()
        {
            Id = 0,
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            Quantity = productDTO.Quantity,
            Categories = productDTO.Categories,
            ImageUrl = productDTO.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        return product;
    }
}
