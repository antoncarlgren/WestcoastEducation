using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminApp.ViewModels.Teachers;

public class AddTeacherCourseViewModel
{
    public TeacherViewModel? Teacher { get; set; }
    public List<SelectListItem> AvailableCourses { get; set; }
    
    public string? SelectedCourseId { get; set; }
}