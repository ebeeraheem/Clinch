using ClinchApi.Data;
using ClinchApi.Extensions;
using ClinchApi.Entities;
using Microsoft.EntityFrameworkCore;
using ClinchApi.Models.DTOs;

namespace ClinchApi.Services;

public class CategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    //Get all categories
    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _context.Categories.AsNoTracking()
            .ToListAsync();
    }

    //Get a category by Id
    public async Task<Category?> GetCategoryById(int id)
    {
        return await _context.Categories.AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == id);
    }

    //Create a new category
    public async Task<Category> Create(CategoryDTO newCategoryDTO)
    {
        var validCategoryDTO = await CategoryValidator
            .ValidateCategory(newCategoryDTO, _context);

        var category = new Category() { Name = validCategoryDTO.Name };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category;
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
            throw new InvalidOperationException($"Category with ID {id} not found");
        }

        var validCategory = await CategoryValidator
            .ValidateCategory(newCategory, _context, true, id);

        categoryToUpdate.Name = validCategory.Name;
        await _context.SaveChangesAsync();
    }

    //Delete a category
    public async Task Delete(int id)
    {
        var categoryToDelete = await _context.Categories.FindAsync(id);
        if (categoryToDelete is null)
        {
            throw new InvalidOperationException($"Category with ID {id} not found");
        }

        _context.Categories.Remove(categoryToDelete);
        await _context.SaveChangesAsync();
    }
}
