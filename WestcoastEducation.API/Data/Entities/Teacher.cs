using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.API.Data.Entities;

public class Teacher : IEntity
{
    [Key]
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    
    public ICollection<Category>? Competencies { get; set; }
    public ICollection<Course> Courses { get; set; }
}