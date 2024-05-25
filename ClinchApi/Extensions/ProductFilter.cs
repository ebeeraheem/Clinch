using ClinchApi.Entities;

namespace ClinchApi.Extensions;

public static class ProductFilter
{
    public static IEnumerable<Product> FilterProducts(
        IEnumerable<Product> products,
        decimal? maxPrice,
        decimal? minPrice,
        int? lowStock,
        int? categoryId,
        DateTime? createdAfter,
        DateTime? createdBefore)
    {
        if (maxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= maxPrice);
        }

        if (minPrice.HasValue)
        {
            products = products.Where(p => p.Price >= minPrice);
        }

        if (lowStock.HasValue)
        {
            products = products.Where(p => p.Quantity <= lowStock);
        }

        if (categoryId.HasValue)
        {
            products = products.Where(
                p => p.Categories.Any(c => c.Id == categoryId));
        }

        if (createdAfter.HasValue)
        {
            products = products.Where(p => p.CreatedAt > createdAfter);
        }

        if (createdBefore.HasValue)
        {
            products = products.Where(p => p.CreatedAt < createdBefore);
        }

        return products;
    }
}
