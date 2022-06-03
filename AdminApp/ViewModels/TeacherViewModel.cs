namespace AdminApp.ViewModels;

public class TeacherViewModel
{
    public string? AppUserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<string> Courses { get; set; } = new();
    public List<string> Competencies { get; set; } = new();
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}