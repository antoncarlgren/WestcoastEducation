using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.ViewModels.Category;

public class CategoryWithTeachersViewModel
{
    public string? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public List<TeacherOverviewViewModel> Teachers { get; set; } = new();
}