namespace StudentApp.ViewModels;

public class CategoryWithCoursesViewModel
{
    public string CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public List<CourseOverviewModel> Courses { get; set; } = new();
}