using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WestcoastEducation.API.Data.Entities;

public class Teacher : IEntity
{
    public string? Id { get; set; }
    
    
    public ApplicationUser ApplicationUser { get; set; }
    public string? ApplicationUserId { get; set; }
    
    public ICollection<TeacherCompetency>? TeacherCompetencies { get; set; }
    public ICollection<Course>? Courses { get; set; }
}