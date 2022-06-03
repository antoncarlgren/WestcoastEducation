namespace AdminApp.ViewModels;

public class CategoryWithTeachersViewModel
{
    public string CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public List<TeacherViewModel> Teachers { get; set; } = new();
}