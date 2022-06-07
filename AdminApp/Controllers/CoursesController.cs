using AdminApp.Models;
using AdminApp.ViewModels;
using AdminApp.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;

namespace AdminApp.Controllers;

[Route("[controller]")]
public class CoursesController : Controller
{
    private readonly CourseServiceModel _courseService;
    private readonly CategoryServiceModel _categoryService;
    private readonly TeacherServiceModel _teacherService;

    public CoursesController(IConfiguration config)
    {
        _courseService = new(config);
        _categoryService = new(config);
        _teacherService = new(config);
    }
    // GET: courses/list
    [HttpGet("list")]
    public async Task<IActionResult> Courses()
    {
        try
        {
            var courses = await _courseService
                .ListCoursesAsync();

            return View("Courses", courses);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // GET: courses/details
    [HttpGet("details")]
    public async Task<IActionResult> Details(CourseViewModel model)
    {
        try
        {
            var course = await _courseService
                .GetCourseByIdAsync(model.Id);

            return View("Details", course);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    [HttpGet("create")]
    public async Task <IActionResult> CreateCourse(AddCourseViewModel model)
    {
        try
        {
            var teachers = await _teacherService
                .ListTeachersAsync();

            var categories = await _categoryService
                .ListCategoriesAsync();

            model.AvailableTeachers = teachers
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Id
                })
                .ToList();

            model.AvailableCategories = categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                })
                .ToList();

            return View("AddCourse", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // POST: courses
    [HttpPost("create")]
    public async Task<IActionResult> AddCourse(AddCourseViewModel model)
    {
        try
        {
            var response = await _courseService
                .PostCourseAsync(model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Courses");
            }

            return View("Error");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    [HttpGet("edit")]
    public IActionResult EditCourse(PatchCourseViewModel model) => View(model);

    // PATCH: courses
    [HttpPost("edit")]
    public async Task<IActionResult> UpdateCourse(PatchCourseViewModel model)
    {
        try
        {
            var response = await _courseService
                .UpdateCourseAsync(model.Id!, model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Courses");
            }

            return View("EditCourse", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    [HttpPost("{id}/delete")]
    public async Task<IActionResult> DeleteCourse(string id)
    {
        try
        {
            var response = await _courseService
                .DeleteCourseAsync(id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Courses");
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