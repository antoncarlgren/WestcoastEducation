using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WestcoastEducation.API.Data.Entities;

public class Student : IdentityUser, IEntity
{
    public ICollection<Course> Courses { get; set; }
}