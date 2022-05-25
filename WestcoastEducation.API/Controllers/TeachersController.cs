using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Controllers;

[ApiController]
[Route("api/v1/teachers")]
public class TeachersController : Controller
{
    private readonly ITeacherRepository _teacherRepository;

    public TeachersController(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }
    
    [HttpGet("list")]
    public async Task<ActionResult<List<TeacherViewModel>>> ListTeachers()
    {
        return Ok(await _teacherRepository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherViewModel>> GetTeacherById(string id)
    {
        var response = await _teacherRepository.GetByIdAsync(id);
        
        if (response is null)
        {
            return NotFound($"No {nameof(Teacher).ToLower()} with id {id} could be found.");
        }

        return Ok(response);
    }

    [HttpPatch("{id}/addcourse")]
    public async Task<ActionResult> AddTeacherToCourse(string id, TeacherCourseViewModel model)
    {
        if (id != model.TeacherId)
        {
            return BadRequest("Invalid teacher id.");
        }
        
        try
        {
            await _teacherRepository.AddCourseAsync(model);

            if (await _teacherRepository.SaveAsync())
            {
                return Ok("Course added successfully.");
            }

            return StatusCode(500, "Could not add course.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPatch("{id}/removecourse")]
    public async Task<ActionResult> RemoveTeacherFromCourse(string id, TeacherCourseViewModel model)
    {
        if (id != model.TeacherId)
        {
            return BadRequest("Invalid teacher id.");
        }
        
        try
        {
            await _teacherRepository.RemoveCourseAsync(model);

            if (await _teacherRepository.SaveAsync())
            {
                return Ok("Course removed successfully.");
            }

            return StatusCode(500, "Could not remove course.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPatch("{id}/addcompentency")]
    public async Task<ActionResult> AddCompetency(string id, TeacherCompetencyViewModel model)
    {
        if (id != model.TeacherId)
        {
            return BadRequest("Invalid teacher id.");
        }
        
        try
        {
            await _teacherRepository.AddCompetencyAsync(model);

            if (await _teacherRepository.SaveAsync())
            {
                return Ok("Teacher competency added successfully.");
            }

            return StatusCode(500, "Teacher competency could not be added.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPatch("{id}/removecompetency")]
    public async Task<ActionResult> RemoveCompentency(string id, TeacherCompetencyViewModel model)
    {
        if (id != model.TeacherId)
        {
            return BadRequest("Invalid teacher id.");
        }
        
        try
        {
            await _teacherRepository.RemoveCompetencyAsync(model);

            if (await _teacherRepository.SaveAsync())
            {
                return Ok("Teacher competency removed successfully.");
            }

            return StatusCode(500, "Teacher competency could not be removed.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}