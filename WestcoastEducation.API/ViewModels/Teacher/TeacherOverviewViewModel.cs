namespace WestcoastEducation.API.ViewModels.Teacher;

public class TeacherViewModel
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<string>? Courses { get; set; }
    public List<string>? Competencies { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}