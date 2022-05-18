using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WestcoastEducation.API.Data.Entities;

public class Student : IdentityUser, IEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<StudentCourse> StudentCourses { get; set; }
}