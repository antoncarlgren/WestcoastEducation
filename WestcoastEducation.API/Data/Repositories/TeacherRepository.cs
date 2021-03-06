using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Authorization;
using WestcoastEducation.API.ViewModels.Category;
using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Data.Repositories;

public class TeacherRepository : RepositoryBase<Teacher, TeacherViewModel, RegisterUserViewModel, PatchApplicationUserViewModel>,
    ITeacherRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public TeacherRepository(ApplicationContext context, IMapper mapper, UserManager<ApplicationUser> userManager) 
        : base(context, mapper)
    {
        _userManager = userManager;
    }

    public async Task<List<CategoryWithTeachersViewModel>> GetCategoriesWithTeachersAsync()
    {
        // Frankenstein's LINQ query
        return await Context.Categories
            .Include(e => e.TeacherCompetencies)
            .ThenInclude(e => e.Teacher)
            .ThenInclude(e => e!.ApplicationUser)
            .Include(e => e.TeacherCompetencies)
            .ThenInclude(e => e.Teacher)
            .ThenInclude(e => e!.Courses)
            .Select(e => new CategoryWithTeachersViewModel
            {
                CategoryId = e.Id,
                CategoryName = e.Name,
                Teachers = e.TeacherCompetencies
                    .Select(tc => new TeacherOverviewViewModel
                    {
                        AppUserId = tc.Teacher!.ApplicationUserId,
                        Name = $"{tc.Teacher!.ApplicationUser!.FirstName} {tc.Teacher.ApplicationUser.LastName}",
                        Email = tc.Teacher.ApplicationUser.Email,
                        PhoneNumber = tc.Teacher.ApplicationUser.PhoneNumber,
                        Address = tc.Teacher.ApplicationUser.Address
                    })
                    .ToList()
            })
            .ToListAsync();
    }
    
    public async Task<string> GetIdByApplicationUserIdAsync(string appUserId)
    {
        var teacher = await Context.Teachers
            .FirstOrDefaultAsync(t => t.ApplicationUserId == appUserId);

        if (teacher is null)
        {
            throw new Exception($"{nameof(Teacher)} with ApplicationUserId {appUserId} could not be found.");
        }

        return teacher.Id!;
    }
    
    public override async Task AddAsync(RegisterUserViewModel model)
    {
        var teacherToAdd = Mapper.Map<Teacher>(model);

        await Context.Teachers.AddAsync(teacherToAdd);
    }

    public override async Task UpdateAsync(string id, RegisterUserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            throw new Exception($"No user with id {id} could be found.");
        }
        
        user.Address = model.Address;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.Email = model.Email;
        user.UserName = model.Email;

        await _userManager.UpdateAsync(user);
    }

    public override async Task UpdateAsync(string id, PatchApplicationUserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            throw new Exception($"No user with id {id} could be found.");
        }
        
        user.Address = model.Address;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.Email = model.Email;
        user.UserName = model.Email;

        await _userManager.UpdateAsync(user);
    }
    
    public async Task AddCompetencyAsync(TeacherCompetencyViewModel model)
    {
        var teacher = await Context.Teachers
            .Include(e => e.TeacherCompetencies)!
            .ThenInclude(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == model.TeacherId);

        if (teacher is null)
        {
            throw new Exception($"No {nameof(Teacher).ToLower()} with id {model.TeacherId} could be found.");
        }

        var competency = await Context.Categories
            .Include(e => e.TeacherCompetencies)
            .FirstOrDefaultAsync(e => e.Id == model.CategoryId);

        if (competency is null)
        {
            throw new Exception($"No {nameof(Category).ToLower()} with id {model.CategoryId} could be found.");
        }
        
        // Only add new competency if teacher doesn't already have it
        if (!HasCompetency(teacher, model.CategoryId!))
        {
            var teacherCompetency = new TeacherCompetency
            {
                TeacherId = model.TeacherId,
                Teacher = teacher,
                CategoryId = model.CategoryId,
                Category = competency
            };
            
            await Context.TeacherCompetencies.AddAsync(teacherCompetency);
            
            teacher.TeacherCompetencies!.Add(teacherCompetency);
            competency.TeacherCompetencies.Add(teacherCompetency);
        }
    }

    public async Task RemoveCompetencyAsync(TeacherCompetencyViewModel model)
    {
        var teacher = await Context.Teachers
            .Include(e => e.TeacherCompetencies)!
            .ThenInclude(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == model.TeacherId);

        if (teacher is null)
        {
            throw new Exception($"No {nameof(Teacher).ToLower()} with id {model.TeacherId} could be found.");
        }

        var competency = await Context.Categories
            .Include(e => e.TeacherCompetencies)
            .FirstOrDefaultAsync(e => e.Id == model.CategoryId);

        if (competency is null)
        {
            throw new Exception($"No {nameof(Category).ToLower()} with id {model.CategoryId} could be found.");
        }

        // Only remove competency if teacher actually has it
        if (HasCompetency(teacher, model.CategoryId!))
        {
            var teacherCompetency = await Context.TeacherCompetencies
                .FirstOrDefaultAsync(e => e.TeacherId == model.TeacherId && e.CategoryId == model.CategoryId);

            teacher.TeacherCompetencies!.Remove(teacherCompetency!);
            Context.TeacherCompetencies.Remove(teacherCompetency!);
        }
    }

    public async Task AddCourseAsync(TeacherCourseViewModel model)
    {
        var teacher = await Context.Teachers
            .Include(e => e.Courses)
            .FirstOrDefaultAsync(e => e.Id == model.TeacherId);
        
        if (teacher is null)
        {
            throw new Exception($"No {nameof(Teacher).ToLower()} with id {model.TeacherId} could be found.");
        }

        var course = await Context.Courses
            .Include(e => e.StudentCourses)
            .FirstOrDefaultAsync(e => e.Id == model.CourseId);

        if (course is null)
        {
            throw new Exception($"No {nameof(Course).ToLower()} with id {model.CourseId} could be found.");
        }

        if (!IsTeachingCourse(teacher, model.CourseId!))
        {
            teacher.Courses!.Add(course);
        }
    }

    public async Task RemoveCourseAsync(TeacherCourseViewModel model)
    {
        var teacher = await Context.Teachers
            .Include(e => e.Courses)
            .FirstOrDefaultAsync(e => e.Id == model.TeacherId);
        
        if (teacher is null)
        {
            throw new Exception($"No {nameof(Teacher).ToLower()} with id {model.TeacherId} could be found.");
        }

        var course = await Context.Courses
            .Include(e => e.StudentCourses)
            .FirstOrDefaultAsync(e => e.Id == model.CourseId);

        if (course is null)
        {
            throw new Exception($"No {nameof(Course).ToLower()} with id {model.CourseId} could be found.");
        }

        if (IsTeachingCourse(teacher, model.CourseId!))
        {
            teacher.Courses!.Remove(course);
        }
    }

    public override async Task DeleteAsync(string id)
    {
        var teacher = await Context.Teachers.FindAsync(id);

        if (teacher is null)
        {
            throw new Exception($"No {nameof(Teacher).ToLower()} with id {id} could be found.");
        }

        var relatedTeacherCompetencies = await Context.TeacherCompetencies
            .Where(e => e.TeacherId == teacher.Id)
            .ToListAsync();
        
        if (relatedTeacherCompetencies is not null)
        {
            Context.TeacherCompetencies
                .RemoveRange(relatedTeacherCompetencies);
        }
        
        Context.Teachers.Remove(teacher);
    }
    
    private static bool HasCompetency(Teacher teacher, string categoryId)
    {
        return teacher.TeacherCompetencies!
            .Any(e => e.CategoryId == categoryId);
    }

    private static bool IsTeachingCourse(Teacher teacher, string courseId)
    {
        return teacher.Courses!
            .Any(e => e.Id == courseId);
    }
}