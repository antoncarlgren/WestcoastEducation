using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.API.Data.Entities;

public class Category : IEntity
{
    [Key] public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public ICollection<Course>? Courses { get; set; }
    public ICollection<TeacherCompetency> TeacherCompetencies { get; set; }
}