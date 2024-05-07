using ClinchApi.Models.Interfaces;
using System.Text.Json.Serialization;

namespace ClinchApi.Models;

public class Category : ICategoryBase
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<Product> Products { get; set; } = new List<Product>();
}
