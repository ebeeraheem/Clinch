namespace ClinchApi.Models;

public class RegisterModel
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public Gender? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    

}
