using System.ComponentModel.DataAnnotations;

namespace ClinchApi.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }
    public List<Category>? Categories { get; set; }
    public Uri ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}
