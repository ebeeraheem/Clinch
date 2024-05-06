namespace ClinchApi.Models.Interfaces;

public interface IProductBase
{
    string Name { get; set; }
    string? Description { get; set; }
    decimal Price { get; set; }
    int Quantity { get; set; }
    public List<int>? CategoryId { get; set; }
    Uri? ImageUrl { get; set; }
}
