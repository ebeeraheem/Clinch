using ClinchApi.Data;
using ClinchApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Extensions;

public class CategoryValidator
{
    public static async Task<T> ValidateCategory<T>(
        T category,
        ApplicationDbContext context,
        bool isUpdate = false,
        int id = 0)
        where T : ICategoryBase
    {
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new ArgumentException("Name cannot be null or empty");
        }

        if (await context.Categories.AnyAsync(
            c => c.Name.ToLower() == category.Name.ToLower() &&
            (!isUpdate || c.Id != id)))
        {
            throw new ArgumentException("Category with the same name already exists");
        }

        return category;
    }
}
