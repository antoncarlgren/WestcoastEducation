using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WestcoastEducation.API.Data.Entities;

public class Student : IEntity
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    public ApplicationUser? ApplicationUser { get; set; }
    public string? ApplicationUserId { get; set; }

    public ICollection<StudentCourse>? StudentCourses { get; set; } = new List<StudentCourse>();
}