using System.ComponentModel.DataAnnotations;

namespace AdminApp.ViewModels;

public class PostCourseViewModel
{
    [Required]
    [Range(0, 9999)]
    public int? CourseNo { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public string? Description { get; set; }
    public string? Length { get; set; }
    public string? Type { get; set; }
    public string? CategoryId { get; set; }
}