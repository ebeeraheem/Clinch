using ClinchApi.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ClinchApi.Models.DTOs;

public class ProductDTO : IProductBase
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    public List<Category>? Categories { get; set; }
    public Uri? ImageUrl { get; set; }
}
