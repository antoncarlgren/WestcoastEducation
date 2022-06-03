using AdminApp.ViewModels;

namespace AdminApp.Models;

public class TeacherServiceModel : ServiceBaseModel
{
    public TeacherServiceModel(IConfiguration config) 
        : base(config, "teachers") { }

    public async Task<List<TeacherViewModel>> ListTeachersAsync()
    {
        return await GetItemsOfTypeAsync<TeacherViewModel>();
    }

    public async Task<List<CategoryWithTeachersViewModel>> GetTeachersByCategoryAsync()
    {
        var response = await HttpGetResponseMessageAsync($"{BaseUrl}/categories");

        var categoriesWithTeachers = await response.Content
            .ReadFromJsonAsync<List<CategoryWithTeachersViewModel>>();

        return categoriesWithTeachers ?? new List<CategoryWithTeachersViewModel>();
    }
    
    public async Task<TeacherViewModel> GetTeacherByIdAsync(string id)
    {
        return await GetByIdAsync<TeacherViewModel>(id);
    }
    
    public async Task<HttpResponseMessage> AddCourseAsync(string id, TeacherCourseViewModel model)
    {
        return await HttpPatchResponseMessageAsync($"{id}/addcourse", model);
    }

    public async Task<HttpResponseMessage> RemoveCourseAsync(string id, TeacherCourseViewModel model)
    {
        return await HttpPatchResponseMessageAsync($"{id}/removecourse", model);
    }

    public async Task<HttpResponseMessage> AddCompetencyAsync(string id, TeacherCategoryViewModel model)
    {
        return await HttpPatchResponseMessageAsync($"{id}/addcompetency", model);
    }

    public async Task<HttpResponseMessage> RemoveCompetencyAsync(string id, TeacherCategoryViewModel model)
    {
        return await HttpPatchResponseMessageAsync($"{id}/removecompetency", model);
    }
    
    public async Task<HttpResponseMessage> DeleteTeacherAsync(string id)
    {
        return await HttpDeleteResponseMessageAsync(id);
    }
}