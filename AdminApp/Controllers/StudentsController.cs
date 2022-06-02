using AdminApp.Models;
using AdminApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace AdminApp.Controllers;

[Route("[controller]")]
public class StudentsController : Controller
{
    private readonly StudentServiceModel _studentService;
    private readonly AuthServiceModel _authServiceModel;

    public StudentsController(IConfiguration config)
    {
        _studentService = new(config);
        _authServiceModel = new(config);
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
    
    // POST: students
    [HttpPost]
    public async Task<IActionResult> RegisterStudent(RegisterUserViewModel model)
    {
        try
        {
            model.IsTeacher = false;

            var response = await _authServiceModel
                .RegisterUserAsync(model);

            if (response.IsSuccessStatusCode)
            {
                return View("Students", null);
            }

            return View("Register", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: students/id/addcourse
    [HttpPatch("{id}/addcourse")]
    public async Task<IActionResult> AddCourse(string id, StudentCourseViewModel model)
    {
        try
        {
            var response = await _studentService
                .AddCourseAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return View("Students", null);
            }

            return View("AddCourse", null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: students/id/removecourse
    [HttpPatch("{id}/removecourse")]
    public async Task<IActionResult> RemoveCourse(string id, StudentCourseViewModel model)
    {
        try
        {
            var response = await _studentService
                .RemoveCourseAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return View("Students", null);
            }

            return View("RemoveCourse", null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
}