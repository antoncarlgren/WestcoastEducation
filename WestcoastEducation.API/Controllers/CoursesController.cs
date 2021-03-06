using Microsoft.AspNetCore.Authorization;
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
    private readonly IStudentRepository _studentRepository;

    public CoursesController(
        ICourseRepository courseRepository,
        IStudentRepository studentRepository)
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }
    
    [HttpGet("list")]
    public async Task<ActionResult<List<CourseOverviewViewModel>>> ListCoursesAsync()
    {
        return Ok(await _courseRepository.GetAllCourseOverviewsAsync());
    }
    
    [HttpGet("categories")]
    public async Task<ActionResult<List<CategoryWithCoursesViewModel>>> ListCoursesByCategoryAsync()
    {
        return Ok(await _courseRepository.GetCategoriesWithCoursesAsync());
    }
    
    [HttpGet("byid/{id}")]
    public async Task<ActionResult<CourseViewModel>> GetCourseDetailsByIdAsync(string id)
    {
        var response = await _courseRepository.GetByIdAsync(id);

        if (response is null)
        {
            return NotFound($"No {nameof(Course).ToLower()} with id {id} could be found.");
        }

        return Ok(response);
    }
    
    [HttpGet("{courseNo:int}")]
    public async Task<ActionResult<CourseViewModel>> GetCourseByCourseNoAsync(int courseNo)
    {
        var response = await _courseRepository.GetCourseByCourseNoAsync(courseNo);

        if (response is null)
        {
            return NotFound($"No {nameof(Course).ToLower()} with course number {courseNo} could be found.");
        }

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult> AddCourseAsync(PostCourseViewModel model)
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
    public async Task<ActionResult> UpdateCourseAsync(string id, PostCourseViewModel model)
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
    public async Task<ActionResult> UpdateCourseAsync(string id, PatchCourseViewModel model)
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
    public async Task<ActionResult> DeleteCourseAsync(string id)
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