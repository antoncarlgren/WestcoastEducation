using AdminApp.ViewModels;

namespace AdminApp.Models;

public class StudentServiceModel : ServiceBaseModel
{
    public StudentServiceModel(IConfiguration config) 
        : base(config, "students") { }

    public async Task<List<StudentViewModel>> ListStudentsAsync()
    {
        return await GetItemsOfTypeAsync<StudentViewModel>();
    }

    public async Task<StudentViewModel> GetStudentByIdAsync(string id)
    {
        return await GetByIdAsync<StudentViewModel>(id);
    }
    
    public async Task<HttpResponseMessage> AddCourseAsync(string id, StudentCourseViewModel model)
    {
        return await HttpPatchResponseMessageAsync($"{id}/addcourse", model);
    }

    public async Task<HttpResponseMessage> RemoveCourseAsync(string id, StudentCourseViewModel model)
    {
        return await HttpPatchResponseMessageAsync($"{id}/removecourse", model);
    }
}