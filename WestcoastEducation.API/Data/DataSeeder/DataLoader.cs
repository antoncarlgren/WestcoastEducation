using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.ViewModels.Category;

namespace WestcoastEducation.API.Data.DataSeeder;

// TODO: Implement data seeder for students, teachers, courses, studentcourses, and teachercompetencies.

public class DataLoader
{
    private readonly ApplicationContext _context;
    private readonly string _jsonBasePath = $"{Directory.GetCurrentDirectory()}/jsonData";

    public DataLoader(ApplicationContext context)
    {
        _context = context;
    }

    public async Task LoadCategories()
    {
        if (await _context.Categories.AnyAsync())
        {
            return;
        }
        
        var categoryData = await File
            .ReadAllTextAsync($"{_jsonBasePath}/categories.json");
        
        var categoryViewModels = JsonSerializer
            .Deserialize<List<CategoryViewModel>>(categoryData);


        var categories = categoryViewModels!
            .Select(vm => new Category
            {
                Name = vm.Name
            });
            
        await _context.AddRangeAsync(categories!);
        await _context.SaveChangesAsync();
    }
    
    public async Task LoadStudents()
    {
        if (await _context.Students.AnyAsync())
        {
            return;
        }
    }
}