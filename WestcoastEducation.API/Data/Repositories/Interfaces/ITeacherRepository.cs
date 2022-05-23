using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface ITeacherRepository
    : IRepository<TeacherViewModel, PostTeacherViewModel, PatchTeacherViewModel>
{
    Task AddCompetency(TeacherCompetencyViewModel model);
    Task RemoveCompetency(TeacherCompetencyViewModel model);
    Task AddCourse(TeacherCourseViewModel model);
    Task RemoveCourse(TeacherCourseViewModel model);
}