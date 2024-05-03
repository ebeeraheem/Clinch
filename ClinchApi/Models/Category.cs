using ClinchApi.Models.Interfaces;

namespace ClinchApi.Models;

public class Category : ICategoryBase
{
    public int Id { get; set; }
    public string Name { get; set; }
}
