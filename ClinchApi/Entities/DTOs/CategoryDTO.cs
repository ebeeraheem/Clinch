using ClinchApi.Entities.Interfaces;

namespace ClinchApi.Entities.DTOs;

public class CategoryDTO : ICategoryBase
{
    public string Name { get; set; } = string.Empty;
}
