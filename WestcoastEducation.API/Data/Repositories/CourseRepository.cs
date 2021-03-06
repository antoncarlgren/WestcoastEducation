using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Category;
using WestcoastEducation.API.ViewModels.Course;

namespace WestcoastEducation.API.Data.Repositories;

public class CourseRepository 
    : RepositoryBase<Course, CourseViewModel, PostCourseViewModel, PatchCourseViewModel>, 
    ICourseRepository
{
    public CourseRepository(ApplicationContext context, IMapper mapper)
        : base(context, mapper) { }

    public async Task<List<CourseOverviewViewModel>> GetAllCourseOverviewsAsync()
    {
        return await Context
            .Courses
            .ProjectTo<CourseOverviewViewModel>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<List<CategoryWithCoursesViewModel>> GetCategoriesWithCoursesAsync()
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
    
    public async Task<CourseViewModel> GetCourseByCourseNoAsync(int courseNo)
    {
        var course = await Context.Courses
            .Include(c => c.Teacher)
            .Include(c => c.Category)
            .Where(e => e.CourseNo == courseNo)
            .ProjectTo<CourseViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (course is null)
        {
            throw new Exception($"Could not find {nameof(Course).ToLower()} with course number {courseNo}.");
        }

        return course;

    }
    
    public override async Task AddAsync(PostCourseViewModel model)
    {
        if (await ExistsByCourseNoAsync((int)model.CourseNo!))
        {
            throw new DuplicateNameException($"{nameof(Course)} with course number {model.CourseNo} already exists.");
        }
        
        var teacher = await Context.Teachers
            .Include(e => e.Courses)
            .FirstOrDefaultAsync(e => e.Id == model.TeacherId);

        var category = await Context.Categories
            .Include(e => e.Courses)
            .FirstOrDefaultAsync(e => e.Id == model.CategoryId);
        
        var courseToAdd = Mapper.Map<Course>(model);
        
        courseToAdd.Teacher = teacher
            ?? throw new Exception($"No {nameof(teacher).ToLower()} with id {model.TeacherId} could be found.");
        
        courseToAdd.Category = category
            ?? throw new Exception($"No {nameof(category).ToLower()} with id {model.CategoryId} could be found.");

        await Context.Courses.AddAsync(courseToAdd);
    }

    public override async Task UpdateAsync(string id, PostCourseViewModel model)
    {
        var course = await Context.Courses.FindAsync(id);

        if (course is null)
        {
            throw new Exception($"Could not find {nameof(course).ToLower()} with id {id}.");
        }

        course.CourseNo = model.CourseNo;
        course.Title = model.Title;
        course.Details = model.Details;
        course.Description = model.Description;
        course.Length = model.Length;
        
        Context.Courses.Update(course);
    }
    
    public override async Task UpdateAsync(string id, PatchCourseViewModel model)
    {
        var course = await Context.Courses.FindAsync(id);

        if (course is null)
        {
            throw new Exception($"Could not find {nameof(course).ToLower()}");
        }

        course.CourseNo = model.CourseNo;
        course.Title = model.Title;
        course.Details = model.Details;
        course.Description = model.Description;
        course.Length = model.Length;

        Context.Courses.Update(course);
    }

    public override async Task DeleteAsync(string id)
    {
        var course = await Context.Courses.FindAsync(id);

        if (course is null)
        {
            throw new Exception($"No {nameof(Course).ToLower()} with id {id} could be found.");
        }

        var relatedStudentCourses = Context.StudentCourses
            .Where(e => e.CourseId == course.Id);

        if (relatedStudentCourses is not null)
        {
            Context.StudentCourses
                .RemoveRange(relatedStudentCourses);
        }

        Context.Courses.Remove(course);
    }
    
    private async Task<bool> ExistsByCourseNoAsync(int courseNo)
    {
        return await Context.Courses.AnyAsync(e => e.CourseNo == courseNo);
    }
}