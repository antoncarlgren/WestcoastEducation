using AdminApp.Models;
using AdminApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace AdminApp.Controllers;

public class CoursesController : Controller
{
    private readonly CourseServiceModel _courseService;

    public CoursesController(IConfiguration config)
    {
        _courseService = new(config);
    }
    // GET: courses/list
    [HttpGet("list")]
    public async Task<IActionResult> Courses()
    {
        try
        {
            var courses = await _courseService
                .ListCoursesAsync();

            return View("Courses");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // GET: courses/courseNo
    [HttpGet("{courseNo:int}")]
    public async Task<IActionResult> Details(int courseNo)
    {
        try
        {
            var course = await _courseService
                .GetCourseByCourseNoAsync(courseNo);

            return View("Details", course);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // POST: courses
    [HttpPost]
    public async Task<IActionResult> PostCourse(PostCourseViewModel model)
    {
        try
        {
            var response = _courseService
                .PostCourseAsync(model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: courses/courseNo
    [HttpPatch("{courseNo:int}")]
    public async Task<IActionResult> UpdateCourse(int courseNo, PatchCourseViewModel model)
    {
        try
        {
            var response = await _courseService
                .UpdateCourseAsync(courseNo, model);

            if (response.IsSuccessStatusCode)
            {
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
}