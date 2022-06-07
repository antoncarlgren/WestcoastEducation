using System.ComponentModel.DataAnnotations;

namespace AdminApp.ViewModels.Auth;

public class RegisterUserViewModel
{
    [Required]
    public string? FirstName { get; set; }
    
    [Required]
    public string? LastName { get; set; }
    
    [Required]
    public string? Password { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }
    
    [Required]
    public string? Address { get; set; }
    
    public bool? IsTeacher { get; set; }
}