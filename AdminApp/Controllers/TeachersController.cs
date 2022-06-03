using AdminApp.Models;
using AdminApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;

namespace AdminApp.Controllers;

[Route("[controller]")]
public class TeachersController : Controller
{
    private readonly TeacherServiceModel _teacherService;
    private readonly AuthServiceModel _authServiceModel;

    
    public TeachersController(IConfiguration config)
    {
        _teacherService = new(config);
        _authServiceModel = new(config);
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
    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        try
        {
            var teacher = await _teacherService
                .GetTeacherByIdAsync(id);

            return View("Details", teacher);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    // POST: teachers
    [HttpPost]
    public async Task<IActionResult> RegisterTeacher(RegisterUserViewModel model)
    {
        try
        {
            model.IsTeacher = true;

            var response = await _authServiceModel
                .RegisterUserAsync(model);

            if (response.IsSuccessStatusCode)
            {
                return View("Teachers", null);
            }

            return View("Register", model);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
        
    // GET: teachers/id/edit
    [HttpGet("{id}/edit")]
    public IActionResult EditTeacher(PatchUserViewModel model)
    {
        return View(model);
    }

    // PATCH: teachers/id
    [HttpPost("{id}")]
    public async Task<IActionResult> PatchTeacher(string id, PatchUserViewModel model)
    {
        try
        {
            var response = await _authServiceModel
                .EditUserAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return View("Teachers", null);
            }

            return View("EditTeacher", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    // PATCH: teachers/addcourse
    [HttpPatch("{id}/addcourse")]
    public async Task<IActionResult> AddCourse(string id, TeacherCourseViewModel model)
    {
        try
        {
            var response = await _teacherService
                .AddCourseAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return View("Teachers", null);
            }

            return View("AddCourse", null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: teachers/id/removecourse
    [HttpPatch("{id}/removecourse")]
    public async Task<IActionResult> RemoveCourse(string id, TeacherCourseViewModel model)
    {
        try
        {
            var response = await _teacherService
                .RemoveCourseAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return View("Teachers", null);
            }

            return View("RemoveCourse", null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: teachers/id/addcompetency
    [HttpPatch("{id}/addcompetency")]
    public async Task<IActionResult> AddCompetency(string id, TeacherCategoryViewModel model)
    {
        try
        {
            var response = await _teacherService
                .AddCompetencyAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return View("Teachers", null);
            }

            return View("AddCompetency", null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }
    
    // PATCH: teachers/id/removecompetency
    [HttpPatch("{id}/removecompetency")]
    public async Task<IActionResult> RemoveCompetency(string id, TeacherCategoryViewModel model)
    {
        try
        {
            var response = await _teacherService
                .RemoveCompetencyAsync(id, model);

            if (response.IsSuccessStatusCode)
            {
                return View("RemoveCompetency", null);
            }

            return View("Teachers", null);
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
            var response = await _authServiceModel
                .DeleteUserAsync(id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Teachers", "Teachers", null); 
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