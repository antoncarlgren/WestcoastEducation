using AutoMapper;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Student;

namespace WestcoastEducation.API.Data.Repositories;

public class StudentRepository 
    : RepositoryBase<Student, StudentViewModel, PostStudentViewModel, PatchStudentViewModel>,
    IStudentRepository
{
    public StudentRepository(ApplicationContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public override async Task AddAsync(PostStudentViewModel model)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(string id, PostStudentViewModel model)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(string id, PatchStudentViewModel model)
    {
        throw new NotImplementedException();
    }
}