using Microsoft.AspNetCore.Identity;

namespace WestcoastEducation.API.Data.Entities;

public class ApplicationUser : IdentityUser, IEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
}