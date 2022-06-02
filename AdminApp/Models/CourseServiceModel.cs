using AdminApp.ViewModels;

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
        var response = await HttpGetResponseMessageAsync($"{BaseUrl}/{courseNo}");

        var course = await response.Content.ReadFromJsonAsync<CourseViewModel>();

        return course!;
    }
    
    public async Task<CourseViewModel> GetCourseByIdAsync(string id)
    {
        return await GetByIdAsync<CourseViewModel>(id);
    }

    public async Task<HttpResponseMessage> PostCourseAsync(PostCourseViewModel model)
    {
        return await HttpPostResponseMessageAsync(BaseUrl, model);
    }

    public async Task<HttpResponseMessage> UpdateCourseAsync(int courseNo, PatchCourseViewModel model)
    {
        return await HttpPatchResponseMessageAsync(courseNo.ToString(), model);
    }
    
    
    public async Task<HttpResponseMessage> DeleteCourseAsync(string id)
    {
        return await HttpDeleteResponseMessageAsync(id);
    }
    
    protected override async Task<TViewModel> GetByIdAsync<TViewModel>(string id)
    {
        var response = await HttpGetResponseMessageAsync($"{BaseUrl}/byid/{id}");

        var course = await response.Content.ReadFromJsonAsync<TViewModel>();

        return course!;
    }
}