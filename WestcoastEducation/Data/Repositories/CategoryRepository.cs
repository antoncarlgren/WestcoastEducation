using System.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Category;

namespace WestcoastEducation.API.Data.Repositories;

public class CategoryRepository 
    : RepositoryBase<Category, CategoryViewModel, PostCategoryViewModel, PatchCategoryViewModel>, 
        ICategoryRepository
{
    public CategoryRepository(ApplicationContext context, IMapper mapper) 
        : base(context, mapper)
    {
        
    }

    public override async Task AddAsync(PostCategoryViewModel model)
    {
        await Context.Categories.AddAsync(Mapper.Map<Category>(model));
    }

    public override async Task UpdateAsync(int id, PostCategoryViewModel model)
    {
        var category = await Context.Categories.FindAsync(id);

        if (category is null)
        {
            throw new Exception($"Could not find {nameof(Category)} with id {id}.");
        }

        category.Name = model.Name;

        Context.Categories.Update(category);
    }

    public override async Task UpdateAsync(int id, PatchCategoryViewModel model)
    {
        if (await ExistsByNameAsync(model.Name))
        {
            throw new DuplicateNameException(model.Name);
        }
        
        var category = await Context.Categories.FindAsync(id);

        if (category is null)
        {
            throw new Exception($"Could not find {nameof(Category)} with id {id}.");
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