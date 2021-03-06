using AutoMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Authorization;
using WestcoastEducation.API.ViewModels.Student;

namespace WestcoastEducation.API.Data.Repositories;

public class StudentRepository 
    : RepositoryBase<Student, StudentViewModel, RegisterUserViewModel, PatchApplicationUserViewModel>,
    IStudentRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentRepository(ApplicationContext context, IMapper mapper, UserManager<ApplicationUser> userManager) 
        : base(context, mapper)
    {
        _userManager = userManager;
    }

    public async Task<string> GetIdByApplicationUserIdAsync(string appUserId)
    {
        var student = await Context.Students
            .FirstOrDefaultAsync(s => s.ApplicationUserId == appUserId);

        if (student is null)
        {
            throw new Exception($"{nameof(Student)} with ApplicationUserId {appUserId} could not be found.");
        }

        return student.Id!;
    }
    
    public override async Task AddAsync(RegisterUserViewModel model)
    {
        var studentToAdd = Mapper.Map<Student>(model);

        await Context.Students.AddAsync(studentToAdd);
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


    public async Task AddCourseAsync(StudentCourseViewModel model)
    {
        var student = await Context.Students
            .Include(e => e.StudentCourses)!
            .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == model.StudentId);
        
        if (student is null)
        {
            throw new Exception($"No {nameof(Student).ToLower()} with id {model.StudentId} could be found.");
        }

        var course = await Context.Courses
            .Include(e => e.StudentCourses)
            .FirstOrDefaultAsync(e => e.Id == model.CourseId);

        if (course is null)
        {
            throw new Exception($"No {nameof(Course).ToLower()} with id {model.CourseId} could be found.");
        }
        
        // Only enroll student if not already enrolled in course
        if (!IsEnrolled(student, model.CourseId!))
        {
            var studentCourse = new StudentCourse
            {
                StudentId = model.StudentId,
                Student = student,
                CourseId = model.CourseId,
                Course = course
            };

            await Context.StudentCourses.AddAsync(studentCourse);
            
            student.StudentCourses!.Add(studentCourse);
            course.StudentCourses!.Add(studentCourse);
        }
    }

    public async Task RemoveCourseAsync(StudentCourseViewModel model)
    {
        var student = await Context.Students
            .Include(e => e.StudentCourses)!
            .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == model.StudentId);
        
        if (student is null)
        {
            throw new Exception($"No {nameof(Student).ToLower()} with id {model.StudentId} could be found.");
        }

        var course = await Context.Courses
            .Include(e => e.StudentCourses)
            .FirstOrDefaultAsync(e => e.Id == model.CourseId);

        if (course is null)
        {
            throw new Exception($"No {nameof(Course).ToLower()} with id {model.CourseId} could be found.");
        }

        // Only remove student from course if currently enrolled
        if (IsEnrolled(student, model.CourseId!))
        {
            var studentCourse = await Context.StudentCourses
                .FirstOrDefaultAsync(e => e.StudentId == model.StudentId && e.CourseId == model.CourseId);

            Context.StudentCourses.Remove(studentCourse!);
        }
    }
    
    public override async Task DeleteAsync(string id)
    {
        var student = await Context.Students.FindAsync(id);

        if (student is null)
        {
            throw new Exception($"No {nameof(Student).ToLower()} with id {id} could be found.");
        }

        var relatedStudentCourses = Context.StudentCourses
            .Where(e => e.StudentId == student.Id);

        if (relatedStudentCourses is not null)
        {
            Context.StudentCourses
                .RemoveRange(relatedStudentCourses);
        }

        Context.Students.Remove(student);
    }
    
    private static bool IsEnrolled(Student student, string courseId)
    {
        return student.StudentCourses!
            .Any(e => e.CourseId == courseId);
    }
}