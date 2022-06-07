using System.ComponentModel.DataAnnotations;

namespace AdminApp.ViewModels.Courses;

public class PatchCourseViewModel
{
    [Required]
    public string? Id { get; set; }
    
    [Required]
    [Range(0, 9999)]
    public int CourseNo { get; set; }
    
    [Required]
    public string? Title { get; set; }
    
    [Required]
    public string? Details { get; set; }
    
    [Required]
    public string? Description { get; set; }
    
    [Required]
    public string? Length { get; set; }
}