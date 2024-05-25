using ClinchApi.Entities.Interfaces;
using System.Text.Json.Serialization;

namespace ClinchApi.Entities;

public class Category : ICategoryBase
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public List<Product> Products { get; set; } = new List<Product>();
}
