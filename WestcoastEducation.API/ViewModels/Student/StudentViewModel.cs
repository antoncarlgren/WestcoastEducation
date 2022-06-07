using WestcoastEducation.API.ViewModels.Course;

namespace WestcoastEducation.API.ViewModels.Student;

public class StudentViewModel
{
    public string? Id { get; set; }
    public string? AppUserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<CourseOverviewViewModel> Courses { get; set; } = new();
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}