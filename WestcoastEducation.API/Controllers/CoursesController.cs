using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Category;
using WestcoastEducation.API.ViewModels.Course;
using WestcoastEducation.API.ViewModels.Student;

namespace WestcoastEducation.API.Controllers;

[ApiController]
[Route("api/v1/courses")]
public class CoursesController : Controller
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStudentRepository _studentRepository;

    public CoursesController(
        ICourseRepository courseRepository, 
        ICategoryRepository categoryRepository,
        IStudentRepository studentRepository)
    {
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
        _studentRepository = studentRepository;
    }
    
    [HttpGet("list")]
    public async Task<ActionResult<List<CourseOverviewViewModel>>> ListCourses()
    {
        return Ok(await _courseRepository.GetAllCourseOverviews());
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<CategoryWithCoursesViewModel>>> ListCoursesByCategory()
    {
        return Ok(await _categoryRepository.GetCategoriesWithCourses());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CourseViewModel>> GetCourseDetailsById(string id)
    {
        var response = await _courseRepository.GetByIdAsync(id);

        if (response is null)
        {
            return NotFound($"No {nameof(Course).ToLower()} with id {id} could be found.");
        }

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult> AddCourse(PostCourseViewModel model)
    {
        try
        {
            await _courseRepository.AddAsync(model);

            if (await _courseRepository.SaveAsync())
            {
                return StatusCode(201);
            }

            return StatusCode(500, $"Could not save {nameof(Course).ToLower()} to database.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCourse(string id, PostCourseViewModel model)
    {
        try
        {
            await _courseRepository.UpdateAsync(id, model);

            if (await _courseRepository.SaveAsync())
            {
                return NoContent();
            }

            return StatusCode(500, $"could not update {nameof(Course).ToLower()}.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateCourse(string id, PatchCourseViewModel model)
    {
        try
        {
            await _courseRepository.UpdateAsync(id, model);

            if (await _courseRepository.SaveAsync())
            {
                return NoContent();
            }

            return StatusCode(500, $"Could not update {nameof(Course).ToLower()}.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCourse(string id)
    {
        try
        {
            await _courseRepository.DeleteAsync(id);

            if (await _courseRepository.SaveAsync())
            {
                return NoContent();
            }

            return StatusCode(500, $"Could not delete {nameof(Course).ToLower()} from database.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}