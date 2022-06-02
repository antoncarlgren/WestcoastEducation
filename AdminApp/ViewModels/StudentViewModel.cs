namespace AdminApp.ViewModels;

public class StudentViewModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<string> Courses { get; set; } = new();
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}