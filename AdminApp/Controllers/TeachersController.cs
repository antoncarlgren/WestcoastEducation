using AdminApp.Models;
using AdminApp.ViewModels;
using AdminApp.ViewModels.Auth;
using AdminApp.ViewModels.Teachers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;

namespace AdminApp.Controllers;

[Route("[controller]")]
public class TeachersController : Controller
{
    private readonly TeacherServiceModel _teacherService;
    private readonly AuthServiceModel _authService;
    private readonly CourseServiceModel _courseService;
    private readonly CategoryServiceModel _categoryService;

    public TeachersController(IConfiguration config)
    {
        _teacherService = new(config);
        _authService = new(config);
        _courseService = new(config);
        _categoryService = new(config);
    }
    // GET: teachers/list
    [HttpGet("list")]
    public async Task<IActionResult> Teachers()
    {
        try
        {
            var teachers = await _teacherService
                .ListTeachersAsync();
            
            return View("Teachers", teachers);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // GET: teachers/competencies
    [HttpGet("competencies")]
    public async Task<IActionResult> Categories()
    {
        try
        {
            var categoriesWithTeachers = await _teacherService
                .GetTeachersByCategoryAsync();

            return View("Categories", categoriesWithTeachers);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }    
    }
    
    // GET: teachers/id
    [HttpGet("details")]
    public async Task<IActionResult> Details(TeacherViewModel model)
    {
        try
        {
            var teacher = await _teacherService
                .GetTeacherByIdAsync(model.Id!);

            return View("Details", teacher);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    [HttpGet("create")]
    public ActionResult RegisterTeacher() => View();

    // POST: teachers
    [HttpPost("create")]
    public async Task<IActionResult> RegisterTeacher(RegisterUserViewModel model)
    {
        try
        {
            model.IsTeacher = true;

            var response = await _authService
                .RegisterUserAsync(model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Teachers");
            }

            return View("RegisterTeacher", model);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
        
    // GET: teachers/edit
    [HttpGet("edit")]
    public IActionResult EditTeacher(PatchUserViewModel model) => View(model);

    // PATCH: teachers/id
    [HttpPost("{id}")]
    public async Task<IActionResult> PatchTeacher(string id, PatchUserViewModel model)
    {
        try
        {
            var response = await _authService
                .EditUserAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Teachers");
            }

            return View("EditTeacher", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    // GET: teachers/id/addcourse
    [HttpGet("addcourse")]
    public async Task<IActionResult> AddTeacherCourse(string id, AddTeacherCourseViewModel model)
    {
        try
        {
            var courses = await _courseService
                .ListCoursesAsync();

            model.Teacher = await _teacherService
                .GetTeacherByIdAsync(id);

            // Ensure teachers can't sign up for courses they are already teaching
            model.AvailableCourses = courses
                .Where(m => model.Teacher.Courses.All(c => c.Id != m.Id))
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
            return RedirectToAction("Details", model.Teacher);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    [HttpPost("addcourse")]
    public async Task<IActionResult> AddTeacherCourse(AddTeacherCourseViewModel model)
    {
        try
        {
            var teacherCourse = new TeacherCourseViewModel
            {
                TeacherId = model.Teacher!.Id,
                CourseId = model.SelectedCourseId
            };

            var response = await _teacherService
                .AddCourseAsync(teacherCourse.TeacherId!, teacherCourse);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Teachers");
            }

            return View("AddTeacherCourse", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: teachers/id/removecourse
    [HttpPost("{id}/removecourse")]
    public async Task<IActionResult> RemoveCourse(string id, TeacherCourseViewModel model)
    {
        try
        {
            var response = await _teacherService
                .RemoveCourseAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Teachers");
            }

            return View("Error");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    [HttpGet("addcompetency")]
    public async Task<IActionResult> AddTeacherCompetency(string id, AddTeacherCategoryViewModel model)
    {
        try
        {
            var categories = await _categoryService
                .ListCategoriesAsync();

            model.Teacher = await _teacherService
                .GetTeacherByIdAsync(id);

            // Ensure teachers cannot be assigned competencies they already have
            model.AvailableCategories = categories
                .Where(m => model.Teacher.Competencies.All(c => c.Id != m.Id))
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                })
                .ToList();

            if (model.AvailableCategories.Any())
            {
                return View(model);
            }
            
            Console.WriteLine("No available competencies found.");
            return RedirectToAction("Details", model.Teacher);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: teachers/addcompetency
    [HttpPost("addcompetency")]
    public async Task<IActionResult> AddTeacherCompetency(AddTeacherCategoryViewModel model)
    {
        try
        {
            var teacherCompetency = new TeacherCategoryViewModel
            {
                TeacherId = model.Teacher!.Id,
                CategoryId = model.SelectedCategoryId
            };

            var response = await _teacherService
                .AddCompetencyAsync(teacherCompetency.TeacherId!, teacherCompetency);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Teachers");
            }
            
            return RedirectToAction("Details", model.Teacher);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: teachers/id/removecompetency
    [HttpPost("{id}/removecompetency")]
    public async Task<IActionResult> RemoveCompetency(string id, TeacherCategoryViewModel model)
    {
        try
        {
            var response = await _teacherService
                .RemoveCompetencyAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Teachers");
            }

            return View("Error");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // DELETE: teachers/id/delete
    [HttpPost("{id}/delete")]
    public async Task<IActionResult> DeleteTeacher(string id)
    {
        try
        {
            var response = await _authService
                .DeleteUserAsync(id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Teachers");
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