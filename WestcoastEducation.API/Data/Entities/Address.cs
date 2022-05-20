namespace WestcoastEducation.API.Data.Entities;

public class Address : IEntity
{
    public string? Id { get; set; }
    public string? City { get; set; }
    public string? StreetName { get; set; }
    public int? ZipCode { get; set; }

    public ICollection<ApplicationUser> ApplicationUsers { get; set; }
}