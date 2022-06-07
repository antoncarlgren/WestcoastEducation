using System.Text.Json;
using StudentApp.ViewModels;

namespace StudentApp.Models;

public class CourseServiceModel : ServiceBaseModel
{
    public CourseServiceModel(IConfiguration config) 
        : base(config, "courses") { }

    public async Task<List<CourseOverviewModel>> ListCoursesAsync()
    {
        return await GetItemsOfTypeAsync<CourseOverviewModel>();
    }

    public async Task<CourseViewModel> GetCourseByCourseNoAsync(int courseNo)
    {
        var response = await OnGetAsync($"{BaseUrl}/{courseNo}");

        var course = await response.Content.ReadFromJsonAsync<CourseViewModel>();

        return course!;
    }

    public async Task<CourseViewModel> GetCourseByIdAsync(string id)
    {
        return await GetByIdAsync<CourseViewModel>(id);
    }

    public async Task<List<CategoryWithCoursesViewModel>> GetCoursesByCategoryAsync()
    {
        var response = await OnGetAsync($"{BaseUrl}/categories");

        var categoriesWithCourses = await response.Content
            .ReadFromJsonAsync<List<CategoryWithCoursesViewModel>>();

        return categoriesWithCourses ?? new List<CategoryWithCoursesViewModel>();
    }

    protected override async Task<TViewModel> GetByIdAsync<TViewModel>(string id)
    {
        var response = await OnGetAsync($"{BaseUrl}/byid/{id}");

        var course = await response.Content.ReadFromJsonAsync<TViewModel>();

        return course!;
    }
}