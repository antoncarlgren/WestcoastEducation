using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.ViewModels.Authorization;
using WestcoastEducation.API.ViewModels.Category;
using WestcoastEducation.API.ViewModels.Course;
using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Data.DataSeeder;

public class DataSeeder
{
    private readonly ApplicationContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly string _jsonBasePath;
    private readonly Random _random;


    public DataSeeder(ApplicationContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
        _random = new Random();
        _jsonBasePath = Path.Combine(Directory.GetCurrentDirectory(), "./Data/DataSeeder/jsonData");
    }

    public async Task LoadCategories()
    {
        if (await _context.Categories.AnyAsync()) return;

        var json = await File
            .ReadAllTextAsync(Path.Combine(_jsonBasePath, "categories.json"));

        var categoryData = JsonSerializer
            .Deserialize<List<PostCategoryViewModel>>(json)!
            .ToArray();

        var categories = categoryData
            .Select(vm => new Category
            {
                Name = vm.Name
            })
            .ToArray();

        await _context.Categories.AddRangeAsync(categories!);
        await _context.SaveChangesAsync();
    }

    public async Task LoadTeachers()
    {
        if (await _context.Teachers.AnyAsync()) return;

        var json = await File
            .ReadAllTextAsync(Path.Combine(_jsonBasePath, "teachers.json"));

        var userData = JsonSerializer
            .Deserialize<List<RegisterUserViewModel>>(json)!
            .ToArray();

        var competencies = await _context.Categories
            .ToListAsync();

        var users = userData
            .Select(vm => new ApplicationUser
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    PhoneNumber = vm.PhoneNumber,
                    Address = vm.Address,
                    Email = vm.Email!.ToLower(),
                    UserName = vm.Email.ToLower()
                })
            .ToArray();

        foreach (var user in users)
        {
            await _userManager.CreateAsync(user);
            
            await _userManager.AddClaimsAsync(user, new Claim[]
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new("Teacher", "true")
            });
        }

        var teachers = users
            .Select(u => new Teacher
                {
                    ApplicationUser = u,
                    ApplicationUserId = u.Id,
                })
            .ToArray();


        foreach (var teacher in teachers)
        {
            
            var teacherCompetencies = new List<TeacherCompetency>();

            // Give each teacher 1-2 random competencies
            for (var j = 0; j < 2; j++)
            {
                var randomIndex = _random.Next(competencies.Count);

                var competency = competencies[randomIndex];
                
                // Check that entity with key does not exist in database bofre adding
                var competencyExists = teacherCompetencies
                    .Any(e => 
                        e.TeacherId == teacher.Id 
                        && e.CategoryId == competency.Id);
                
                if (competencyExists) continue;
                
                var teacherCompetency = new TeacherCompetency
                {
                    TeacherId = teacher.Id,
                    CategoryId = competency.Id
                };
                
                teacherCompetencies.Add(teacherCompetency);
            }

            teacher.TeacherCompetencies = teacherCompetencies;

            await _context.TeacherCompetencies.AddRangeAsync();
        }

        await _context.Teachers.AddRangeAsync(teachers);
        await _context.SaveChangesAsync();
    }

    public async Task LoadStudents()
    {
        if (await _context.Students.AnyAsync()) return;

        var json = await File
            .ReadAllTextAsync(Path.Combine(_jsonBasePath, "students.json"));

        var userData = JsonSerializer
            .Deserialize<List<RegisterUserViewModel>>(json)!
            .ToArray();

        var users = userData
            .Select(vm => new ApplicationUser
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Address = vm.Address,
                    PhoneNumber = vm.PhoneNumber,
                    Email = vm.Email!.ToLower(),
                    UserName = vm.Email.ToLower()
                })
            .ToArray();

        foreach (var user in users)
        {
            await _userManager.CreateAsync(user);
            
            await _userManager.AddClaimsAsync(user, new Claim[]
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email)
            });
        }

        var students = users
            .Select(u => new Student
                {
                    ApplicationUser = u,
                    ApplicationUserId = u.Id
                })
            .ToArray();

        await _context.Students.AddRangeAsync(students);
        await _context.SaveChangesAsync();
    }

    public async Task LoadCourses()
    {
        if (await _context.Courses.AnyAsync()) return;

        var json = await File
            .ReadAllTextAsync(Path.Combine(_jsonBasePath, "courses.json"));

        var courseData = JsonSerializer
            .Deserialize<List<PostCourseViewModel>>(json)!
            .ToArray();

        var categories = await _context.Categories
            .ToListAsync();

        var teachers = await _context.Teachers
            .Include(e => e.TeacherCompetencies)!
            .ThenInclude(e => e.Category)
            .ToListAsync();

        var students = await _context.Students
            .ToListAsync();

        // TODO: Refactor this spaghetti
        // spaghetti code deluxe
        // send help
        var courses = courseData
            .Select(vm =>
            {
                var category = categories
                    .FirstOrDefault(c =>
                        string.Equals(c.Name, vm.CategoryName, StringComparison.CurrentCultureIgnoreCase));

                // Ensure teachers can only teach classes for which they have the relevant competency
                var validTeachers = teachers
                    .Where(t =>
                        t.TeacherCompetencies!.Any(tc => tc.CategoryId == category!.Id))
                    .ToArray();
                    
                var teacher = validTeachers[_random.Next(validTeachers.Length)];
                
                var course = new Course
                {
                    CourseNo = vm.CourseNo,
                    Title = vm.Title,
                    Details = vm.Details,
                    Description = vm.Description,
                    Length = vm.Length,
                    Category = category,
                    CategoryId = category!.Id,
                    Teacher = teacher,
                    TeacherId = teacher.Id
                };
                
                teacher.Courses!.Add(course);

                return course;
            })
            .ToList();

        foreach (var student in students)
        {
            var studentCourses = new List<StudentCourse>();
            
            for (var i = 0; i < 2; i++)
            {
                var course = courses[_random.Next(courses.Count)];

                var studentCourseExists = studentCourses
                    .Any(e =>
                        e.StudentId == student.Id
                        && e.CourseId == course.Id);
                
                if(studentCourseExists) continue;

                var studentCourse = new StudentCourse
                {
                    StudentId = student.Id,
                    Student = student,
                    CourseId = course.Id,
                    Course = course
                };
                
                studentCourses.Add(studentCourse);
            }

            student.StudentCourses = studentCourses;
            _context.StudentCourses.AddRange(studentCourses);
        }

        await _context.Courses.AddRangeAsync(courses);
        await _context.SaveChangesAsync();
    }
}