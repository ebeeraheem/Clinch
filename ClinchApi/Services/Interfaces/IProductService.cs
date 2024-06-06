using ClinchApi.Entities;
using ClinchApi.Models.DTOs;

namespace ClinchApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> Create(ProductDTO newProductDTO);
        Task Delete(int id);
        Task<Product?> GetProductById(int id);
        Task<IEnumerable<Product>> GetProducts();
        Task Update(int id, ProductUpdateDTO productUpdateDTO);
    }
}