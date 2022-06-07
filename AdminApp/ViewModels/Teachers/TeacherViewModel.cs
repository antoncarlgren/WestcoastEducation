using AdminApp.ViewModels.Categories;
using AdminApp.ViewModels.Courses;

namespace AdminApp.ViewModels.Teachers;

public class TeacherViewModel
{
    public string? Id { get; set; }
    public string? AppUserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<CourseOverviewModel> Courses { get; set; } = new();
    public List<CategoryViewModel> Competencies { get; set; } = new();
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}