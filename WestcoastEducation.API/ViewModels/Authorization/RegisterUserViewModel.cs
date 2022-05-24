namespace WestcoastEducation.API.ViewModels.Authorization;

public class RegisterUserViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public bool IsTeacher { get; set; } = false;
}