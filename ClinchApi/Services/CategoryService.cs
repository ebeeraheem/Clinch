using ClinchApi.Data;
using ClinchApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinchApi.Services;

public class CategoryService
{
    private readonly EcommerceDbContext _context;

    public CategoryService(EcommerceDbContext context)
    {
        _context = context;
    }

    //Get all categories
    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }

    //Get a category by Id
    public async Task<Category?> GetCategoryById(int id)
    {
        return await _context.Categories.AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == id);
    }

    //Create a new category
    public async Task<Category> Create(Category newCategory)
    {
        if (newCategory is null)
        {
            throw new ArgumentNullException(nameof(newCategory));
        }

        if (string.IsNullOrWhiteSpace(newCategory.Name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(newCategory.Name));
        }

        if (await _context.Categories.AnyAsync(c => c.Name == newCategory.Name))
        {
            throw new InvalidOperationException("Category with the same name already exists");
        }

        _context.Categories.Add(newCategory);
        await _context.SaveChangesAsync();

        return newCategory;
    }

    //Update a category
    public async Task Update(int id, Category newCategory)
    {
        if (newCategory is null || newCategory.Id != id)
        {
            throw new ArgumentException("Invalid ID or category");
        }

        var categoryToUpdate = await _context.Categories.FindAsync(id);
        if (categoryToUpdate is null)
        {
            throw new InvalidOperationException("Category not found");
        }

        if (string.IsNullOrWhiteSpace(newCategory.Name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(newCategory.Name));
        }

        if (await _context.Categories.AnyAsync(c => c.Name == newCategory.Name && c.Id != id))
        {
            throw new InvalidOperationException("Category with the same name already exists");
        }

        _context.Entry(categoryToUpdate).CurrentValues.SetValues(newCategory);
        await _context.SaveChangesAsync();
    }

    //Remove a category
    public async Task Delete(int id)
    {
        var categoryToDelete = await _context.Categories.FindAsync(id);
        if (categoryToDelete is null)
        {
            throw new InvalidOperationException("Category not found");
        }

        _context.Categories.Remove(categoryToDelete);
        await _context.SaveChangesAsync();
    }
}
