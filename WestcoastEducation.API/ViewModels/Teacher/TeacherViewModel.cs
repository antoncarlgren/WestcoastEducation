using WestcoastEducation.API.ViewModels.Category;
using WestcoastEducation.API.ViewModels.Course;

namespace WestcoastEducation.API.ViewModels.Teacher;

public class TeacherViewModel
{
    public string? Id { get; set; }
    public string? AppUserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<CourseOverviewViewModel>? Courses { get; set; }
    public List<CategoryViewModel>? Competencies { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    
}