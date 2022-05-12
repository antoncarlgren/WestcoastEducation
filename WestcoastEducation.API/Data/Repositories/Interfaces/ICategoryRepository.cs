using WestcoastEducation.API.ViewModels.Category;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface ICategoryRepository 
    : IRepository<CategoryViewModel, PostCategoryViewModel, PatchCategoryViewModel>
{
    
}