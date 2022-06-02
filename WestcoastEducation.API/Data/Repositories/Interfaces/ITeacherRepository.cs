using WestcoastEducation.API.ViewModels.Authorization;
using WestcoastEducation.API.ViewModels.Category;
using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface ITeacherRepository
    : IRepository<TeacherViewModel, RegisterUserViewModel, PatchApplicationUserViewModel>
{
    Task<string> GetIdByApplicationUserIdAsync(string appUserId);
    Task AddCompetencyAsync(TeacherCompetencyViewModel model);
    Task RemoveCompetencyAsync(TeacherCompetencyViewModel model);
    Task AddCourseAsync(TeacherCourseViewModel model);
    Task RemoveCourseAsync(TeacherCourseViewModel model);
    Task<List<CategoryWithTeachersViewModel>> GetCategoriesWithTeachersAsync();
}