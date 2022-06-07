using System.ComponentModel.DataAnnotations;
using AdminApp.ViewModels.Categories;
using AdminApp.ViewModels.Teachers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminApp.ViewModels.Courses;

public class AddCourseViewModel
{
    [Required]
    public string? Title { get; set; }
    [Required]
    [Range(0, 9999)]
    public int? CourseNo { get; set; }
    [Required]
    public string? Details { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public string? Length { get; set; }
    [Required]
    public string? TeacherId { get; set; }
    [Required]
    public string? CategoryId { get; set; }

    public List<SelectListItem> AvailableTeachers { get; set; } = new();
    public List<SelectListItem> AvailableCategories { get; set; } = new();
}