﻿using ClinchApi.Entities;

namespace ClinchApi.Models;

public class UpdateUserModel
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Gender? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}
