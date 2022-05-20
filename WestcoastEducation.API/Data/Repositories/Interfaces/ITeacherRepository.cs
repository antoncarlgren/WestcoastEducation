using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface ITeacherRepository
    : IRepository<TeacherViewModel, PostTeacherViewModel, PatchTeacherViewModel>
{
    
}