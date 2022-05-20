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

    public async Task<List<CategoryWithCoursesViewModel>> GetCategoriesWithCourses()
    {
        return await Context.Categories
            .Include(e => e.Courses)
            .Select(e => new CategoryWithCoursesViewModel
            {
                CategoryId = e.Id,
                CategoryName = e.Name,
                Courses = e.Courses!
                    .Select(c => new CourseOverviewViewModel
                    {
                        Title = c.Title,
                        CourseNo = c.CourseNo,
                        Length = c.Length
                    })
                    .ToList()
            })
            .ToListAsync();
    }
    
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

    private async Task<bool> ExistsByNameAsync(string name)
    {
        return await Context.Categories
            .AnyAsync(e => string.Equals(e.Name, name, StringComparison.CurrentCultureIgnoreCase));
    }
}