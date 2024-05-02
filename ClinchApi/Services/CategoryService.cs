using ClinchApi.Data;
using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        return await _context.Categories.AsNoTracking().ToListAsync();
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
        var validCategory = await ValidateCategory(newCategoryDTO, _context);

        var category = DTOToCategory(validCategory);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        
        return category;
    }

    //Update a category
    public async Task Update(int id, Category newCategory)
    {
        //Find a way to check whether the category to update
        //is the same as the provided id
        if (newCategory is null || newCategory.Id != id)
        {
            throw new ArgumentException("Invalid ID or category");
        }
        if (string.IsNullOrWhiteSpace(newCategory.Name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(newCategory.Name));
        }
        if (await _context.Categories.AnyAsync(
            c => c.Name.ToLower() == newCategory.Name.ToLower() &&  
            c.Id != id))
        {
            throw new ArgumentException("Category with the same name already exists");
        }
        //var validCategoryDTO = await ValidateCategory(newCategory, _context, true, id);
        //var category = DTOToCategory(validCategoryDTO);

        var categoryToUpdate = await _context.Categories.FindAsync(id);
        if (categoryToUpdate is null)
        {
            throw new InvalidOperationException($"Category with ID {id} not found");
        }

        //_context.Entry(categoryToUpdate).CurrentValues.SetValues(category);
        categoryToUpdate.Name = newCategory.Name;
        await _context.SaveChangesAsync();
    }

    //Remove a category
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

    /* *************************************** */

    public static async Task<CategoryDTO> ValidateCategory(
        CategoryDTO categoryDTO, 
        ApplicationDbContext context, 
        bool isUpdate = false, 
        int id = 0)
    {
        if (categoryDTO is null)
        {
            throw new ArgumentNullException(nameof(categoryDTO));
        }

        if (string.IsNullOrWhiteSpace(categoryDTO.Name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(categoryDTO.Name));
        }

        if (await context.Categories.AnyAsync(
            c => c.Name.ToLower() == categoryDTO.Name.ToLower() && 
            (isUpdate ? c.Id != id : true)))
        {
            throw new ArgumentException("Category with the same name already exists");
        }

        return categoryDTO;
    }
    public static Category DTOToCategory(CategoryDTO categoryDTO)
    {
        var category = new Category()
        {
            Id = 0,
            Name = categoryDTO.Name
        };
        return category;
    }
}
