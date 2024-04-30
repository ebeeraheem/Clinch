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
        var validCategory = await ValidateCategory(newCategory, _context);

        await _context.Categories.AddAsync(validCategory);
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
        var validCategory = ValidateCategory(newCategory, _context, true, id);

        var categoryToUpdate = await _context.Categories.FindAsync(id);
        if (categoryToUpdate is null)
        {
            throw new InvalidOperationException("Category not found");
        }

        _context.Entry(categoryToUpdate).CurrentValues.SetValues(validCategory);
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

    /* *************************************** */

    public static async Task<Category> ValidateCategory(Category category, EcommerceDbContext context, bool isUpdate = false, int id = 0)
    {
        if (category is null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(category.Name));
        }

        if (await context.Categories.AnyAsync(c => c.Name == category.Name && (isUpdate ? c.Id != id : true)))
        {
            throw new InvalidOperationException("Category with the same name already exists");
        }

        return category;
    }
}
