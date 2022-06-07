using AdminApp.ViewModels;
using AdminApp.ViewModels.Courses;

namespace AdminApp.Models;

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

    public async Task<HttpResponseMessage> PostCourseAsync(AddCourseViewModel model)
    {
        return await OnPostAsync(BaseUrl, model);
    }

    public async Task<HttpResponseMessage> UpdateCourseAsync(string id, PatchCourseViewModel model)
    {
        return await OnPatchAsync(id, model);
    }
    
    
    public async Task<HttpResponseMessage> DeleteCourseAsync(string id)
    {
        return await OnDeleteAsync(id);
    }
    
    protected override async Task<TViewModel> GetByIdAsync<TViewModel>(string id)
    {
        var response = await OnGetAsync($"{BaseUrl}/byid/{id}");

        var course = await response.Content.ReadFromJsonAsync<TViewModel>();

        return course!;
    }
}