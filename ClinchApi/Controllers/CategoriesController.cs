using ClinchApi.Data;
using ClinchApi.Models;
using ClinchApi.Models.DTOs;
using ClinchApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>A list of Category objects</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Category>> GetAsync()
    {
        return await _categoryService.GetCategories();
    }

    /// <summary>
    /// Get a category based on the provided ID
    /// </summary>
    /// <param name="id">The ID of the category to find</param>
    /// <returns>A category with the specified ID</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Category?>> GetByIdAsync(int id)
    {
        var category = await _categoryService.GetCategoryById(id);

        return category == null ?
            NotFound($"Category with ID {id} not found") :
            Ok(category);
    }

    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <param name="newCategoryDTO">The new category to create</param>
    /// <returns>A 'Location' response header with the URL of the newly created category</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Category>> CreateCategoryAsync(CategoryDTO newCategoryDTO)
    {
        try
        {
            var created = await _categoryService.Create(newCategoryDTO);

            //return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
            //The above method returns a 500 Internal Server Error
            //even though the resource was successfully created
            var uri = Url.Action(nameof(GetByIdAsync), new { id = created.Id });
            return Created(uri, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Update a category
    /// </summary>
    /// <param name="id">The ID of the category to update</param>
    /// <param name="newCategoryDTO">The updated category data</param>
    /// <returns>NoContent()</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCategoryAsync(int id, Category newCategory)
    {
        try
        {
            await _categoryService.Update(id, newCategory);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Deletes a category
    /// </summary>
    /// <param name="id">ID of the category to be deteled</param>
    /// <returns>No content</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _categoryService.Delete(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }
}
