using WestcoastEducation.API.ViewModels.Course;

namespace WestcoastEducation.API.ViewModels.Category;

public class CategoryWithCoursesViewModel
{
    public string? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public List<CourseOverviewViewModel> Courses { get; set; } = new();
}