using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdminApp.ViewModels.Auth;

public class PatchUserViewModel
{
    [Required]
    [JsonIgnore]
    public string Id { get; set; }
    
    [Required]
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
    
    [Required]
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [Required] 
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    
    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [Required]
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
}