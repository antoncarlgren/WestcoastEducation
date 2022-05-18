namespace WestcoastEducation.API.ViewModels.Course;

public class PostCourseViewModel
{
    public int? CourseNo { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public string? Description { get; set; }
    public string? Length { get; set; }
    public string? CategoryId { get; set; }
    public string? TeacherId { get; set; }
}