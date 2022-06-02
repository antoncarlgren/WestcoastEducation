using System.ComponentModel.DataAnnotations;

namespace AdminApp.ViewModels;

public class PatchUserViewModel
{
    [Required] 
    public string Id { get; set; }
    
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
}