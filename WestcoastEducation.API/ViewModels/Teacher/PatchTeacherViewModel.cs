﻿namespace WestcoastEducation.API.ViewModels.Teacher;

public class PatchTeacherViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public List<string>? Competencies { get; set; } = new();
}