using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WestcoastEducation.API.Data.Entities;

public class Teacher : IdentityUser, IEntity
{
    public ICollection<Category>? Competencies { get; set; }
    public ICollection<Course> Courses { get; set; }
}