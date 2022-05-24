using WestcoastEducation.API.ViewModels.Authorization;
using WestcoastEducation.API.ViewModels.Student;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface IStudentRepository
    : IRepository<StudentViewModel, RegisterUserViewModel, PatchStudentViewModel>
{
    Task AddCourseAsync(StudentCourseViewModel model);
    Task RemoveCourseAsync(StudentCourseViewModel model);
}