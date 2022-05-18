using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WestcoastEducation.API.Data.Entities;

public class Teacher : IdentityUser, IEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<TeacherCompetency>? TeacherCompetencies { get; set; }
    public ICollection<Course>? Courses { get; set; }
}