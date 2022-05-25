using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Category;
using WestcoastEducation.API.ViewModels.Course;

namespace WestcoastEducation.API.Data.Repositories;

public class CategoryRepository 
    : RepositoryBase<Category, CategoryViewModel, PostCategoryViewModel, PatchCategoryViewModel>, 
    ICategoryRepository
{
    public CategoryRepository(ApplicationContext context, IMapper mapper) 
        : base(context, mapper) { }
    
    public override async Task AddAsync(PostCategoryViewModel model)
    {
        await Context.Categories.AddAsync(Mapper.Map<Category>(model));
    }

    public override async Task UpdateAsync(string id, PostCategoryViewModel model)
    {
        var category = await Context.Categories.FindAsync(id);

        if (category is null)
        {
            throw new Exception($"Could not find {nameof(Category).ToLower()} with id {id}.");
        }

        category.Name = model.Name;

        Context.Categories.Update(category);
    }

    public override async Task UpdateAsync(string id, PatchCategoryViewModel model)
    {
        var category = await Context.Categories.FindAsync(id);

        if (category is null)
        {
            throw new Exception($"Could not find {nameof(Category).ToLower()} with id {id}.");
        }

        category.Name = model.Name;

        Context.Categories.Update(category);
    }

    public override async Task DeleteAsync(string id)
    {
        var relatedCourses = await Context.Courses
            .Where(c => c.CategoryId == id)
            .ToListAsync();

        if (relatedCourses is not null)
        {
            relatedCourses.ForEach(c => c.CategoryId = null);
        }

        var relatedTeacherCompetencies = await Context.TeacherCompetencies
            .Where(tc => tc.CategoryId == id)
            .ToListAsync();

        if (relatedTeacherCompetencies is not null)
        {
            Context.TeacherCompetencies.RemoveRange(relatedTeacherCompetencies);
        }
        
        await base.DeleteAsync(id);
    }
}