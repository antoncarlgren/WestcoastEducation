using AdminApp.Models;
using AdminApp.ViewModels;
using AdminApp.ViewModels.Auth;
using AdminApp.ViewModels.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AdminApp.Controllers;

[Route("[controller]")]
public class StudentsController : Controller
{
    private readonly StudentServiceModel _studentService;
    private readonly AuthServiceModel _authService;
    private readonly CourseServiceModel _courseService;

    public StudentsController(IConfiguration config)
    {
        _courseService = new(config);
        _studentService = new(config);
        _authService = new(config);
    }

    // GET: students/list
    [HttpGet("list")]
    public async Task<IActionResult> Students()
    {
        try
        {
            var students = await _studentService
                .ListStudentsAsync();

            return View("Students", students);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    // GET: students/id
    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        try
        {
            var student = await _studentService
                .GetStudentByIdAsync(id);

            return View("Details", student);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    [HttpGet("edit")]
    public ActionResult EditStudent(PatchUserViewModel model) => View(model);
    
    [HttpPost("edit")]
    public async Task<IActionResult> PatchStudent(string id, PatchUserViewModel model)
    {
        try
        {
            var response = await _authService
                .EditUserAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Students");
            }

            return View("EditStudent", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }        
    }

    [HttpGet("create")]
    public ActionResult RegisterStudent() => View();
    
    // POST: students
    [HttpPost("create")]
    public async Task<IActionResult> RegisterStudent(RegisterUserViewModel model)
    {
        try
        {
            model.IsTeacher = false;

            var response = await _authService
                .RegisterUserAsync(model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Students");
            }

            return View("RegisterStudent", model); 
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    // GET: students/addcourse
    [HttpGet("addcourse")]
    public async Task<IActionResult> AddStudentCourse(string id, AddStudentCourseViewModel model)
    {
        try
        {
            var courses = await _courseService
                .ListCoursesAsync();

            model.Student = await _studentService
                .GetStudentByIdAsync(id);

            model.AvailableCourses = courses
                .Where(m => model.Student.Courses.All(c => c.Id != m.Id))
                .Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id
                })
                .ToList();

            if (model.AvailableCourses.Any())
            {
                return View(model);
            }
            
            Console.WriteLine("No available courses found.");
            return RedirectToAction("Details", model.Student);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: students/addcourse
    [HttpPost("addcourse")]
    public async Task<IActionResult> AddStudentCourse(AddStudentCourseViewModel model)
    {
        try
        {
            var studentCourse = new StudentCourseViewModel
            {
                StudentId = model.Student!.Id,
                CourseId = model.SelectedCourseId
            };
            
            var response = await _studentService
                .AddCourseAsync(studentCourse.StudentId!, studentCourse);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Students");
            }

            return View("AddStudentCourse", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: students/id/removecourse
    [HttpPost("removecourse")]
    public async Task<IActionResult> RemoveStudentCourse(StudentCourseViewModel model)
    {
        try
        {
            var response = await _studentService
                .RemoveCourseAsync(model.StudentId!, model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Students");
            }

            return View("Error");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
   
    // DELETE: students/id/delete
    [HttpPost("{id}/delete")]
    public async Task<IActionResult> DeleteStudent(string id)
    {
        try
        {
            var response = await _authService
                .DeleteUserAsync(id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Students");
            }

            return View("Error");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
}