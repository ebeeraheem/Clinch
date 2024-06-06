using ClinchApi.Entities;
using ClinchApi.Models.DTOs;

namespace ClinchApi.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> Create(CategoryDTO newCategoryDTO);
        Task Delete(int id);
        Task<IEnumerable<Category>> GetCategories();
        Task<Category?> GetCategoryById(int id);
        Task Update(int id, Category newCategory);
    }
}