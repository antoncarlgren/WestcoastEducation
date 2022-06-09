using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminApp.ViewModels.Students;

public class AddStudentCourseViewModel
{
    public StudentViewModel? Student { get; set; }
    public List<SelectListItem> AvailableCourses { get; set; }
        
    public string? SelectedCourseId { get; set; }
}