using AutoMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Student;

namespace WestcoastEducation.API.Data.Repositories;

public class StudentRepository 
    : RepositoryBase<Student, StudentViewModel, PostStudentViewModel, PatchStudentViewModel>,
    IStudentRepository
{
    public StudentRepository(ApplicationContext context, IMapper mapper) 
        : base(context, mapper) { }

    public override async Task AddAsync(PostStudentViewModel model)
    {
        var studentToAdd = Mapper.Map<Student>(model);

        await Context.Students.AddAsync(studentToAdd);
    }

    public override async Task UpdateAsync(string id, PostStudentViewModel model)
    {
        var student = await Context.Students
            .Include(e => e.ApplicationUser)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (student is null)
        {
            throw new Exception($"Could not find {nameof(Student).ToLower()} with id {id}.");
        }

        student.ApplicationUser!.Address = model.Address;
        student.ApplicationUser.FirstName = model.FirstName;
        student.ApplicationUser.LastName = model.LastName;
        student.ApplicationUser.PhoneNumber = model.PhoneNumber;
        student.ApplicationUser.Email = model.Email;

        Context.Students.Update(student);
    }

    public override async Task UpdateAsync(string id, PatchStudentViewModel model)
    {
        var student = await Context.Students
            .Include(e => e.ApplicationUser)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (student is null)
        {
            throw new Exception($"Could not find {nameof(Student).ToLower()} with id {id}.");
        }

        student.ApplicationUser!.Address = model.Address;
        student.ApplicationUser.FirstName = model.FirstName;
        student.ApplicationUser.LastName = model.LastName;
        student.ApplicationUser.PhoneNumber = model.PhoneNumber;
        student.ApplicationUser.Email = model.Email;

        Context.Students.Update(student);
    }

    public async Task AddCourse(StudentCourseViewModel model)
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

    public async Task RemoveCourse(StudentCourseViewModel model)
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

    private static bool IsEnrolled(Student student, string courseId)
    {
        return student.StudentCourses!
            .Any(e => e.CourseId == courseId);
    }
}