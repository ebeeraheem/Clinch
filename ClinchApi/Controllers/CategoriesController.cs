﻿using ClinchApi.Entities;
using ClinchApi.Models.DTOs;
using ClinchApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinchApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>A list of Category objects</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Category>> GetAllCategories()
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
    public async Task<ActionResult<Category?>> GetById(int id)
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
    [Authorize(Roles = "Admin,Store Owner,Store Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Category>> Create([FromBody] CategoryDTO newCategoryDTO)
    {
        try
        {
            var category = await _categoryService.Create(newCategoryDTO);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
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
    /// <param name="newCategory">The updated category data</param>
    /// <returns>No content</returns>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Store Owner,Store Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] Category newCategory)
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
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Store Owner")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
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
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An unexpected error occurred");
        }
    }
}
