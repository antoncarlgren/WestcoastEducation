using AutoMapper;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Data.Repositories;

public class TeacherRepository : RepositoryBase<Teacher, TeacherViewModel, PostTeacherViewModel, PatchTeacherViewModel>,
    ITeacherRepository
{
    public TeacherRepository(ApplicationContext context, IMapper mapper) 
        : base(context, mapper) { }

    public override async Task AddAsync(PostTeacherViewModel model)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(string id, PostTeacherViewModel model)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(string id, PatchTeacherViewModel model)
    {
        throw new NotImplementedException();
    }
}