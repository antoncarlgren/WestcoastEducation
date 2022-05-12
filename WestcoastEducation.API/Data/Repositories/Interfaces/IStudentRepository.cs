using WestcoastEducation.API.ViewModels.Student;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface IStudentRepository
    : IRepository<StudentViewModel, PostStudentViewModel, PatchStudentViewModel>
{
    
}