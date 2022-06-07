using System.Text;
using System.Text.Json;
using AdminApp.ViewModels;
using AdminApp.ViewModels.Categories;

namespace AdminApp.Models;

public class CategoryServiceModel : ServiceBaseModel
{
    public CategoryServiceModel(IConfiguration config) 
        : base(config, "categories") { }

    public async Task<List<CategoryViewModel>> ListCategoriesAsync()
    {
        return await GetItemsOfTypeAsync<CategoryViewModel>();
    }
}