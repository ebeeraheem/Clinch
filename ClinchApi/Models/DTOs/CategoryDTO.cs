using ClinchApi.Models.Interfaces;

namespace ClinchApi.Models.DTOs;

public class CategoryDTO : ICategoryBase
{
    public string Name { get; set; }
}
