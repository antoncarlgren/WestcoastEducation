using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminApp.ViewModels.Teachers;

public class AddTeacherCategoryViewModel
{
        public TeacherViewModel? Teacher { get; set; }
        public List<SelectListItem> AvailableCategories { get; set; }
        
        public string? SelectedCategoryId { get; set; }
}