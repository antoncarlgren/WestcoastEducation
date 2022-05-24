using WestcoastEducation.API.ViewModels.Authorization;
using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface ITeacherRepository
    : IRepository<TeacherViewModel, RegisterUserViewModel, PatchTeacherViewModel>
{
    Task AddCompetencyAsync(TeacherCompetencyViewModel model);
    Task RemoveCompetencyAsync(TeacherCompetencyViewModel model);
    Task AddCourseAsync(TeacherCourseViewModel model);
    Task RemoveCourseAsync(TeacherCourseViewModel model);
}