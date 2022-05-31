using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Student;

namespace WestcoastEducation.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/students")]
public class StudentController : Controller
{
    private readonly IStudentRepository _studentRepository;

    public StudentController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    
    [HttpGet("list")]
    public async Task<ActionResult<List<StudentViewModel>>> ListStudents()
    {
        return Ok(await _studentRepository.GetAllAsync());
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentViewModel>> GetStudentById(string id)
    {
        var response = await _studentRepository.GetByIdAsync(id);

        if (response is null)
        {
            return NotFound($"No {nameof(Student).ToLower()} with id {id} could be found.");
        }

        return Ok(response);
    }
    
    [HttpPatch("{id}/enroll")]
    public async Task<ActionResult> EnrollStudent(string id, StudentCourseViewModel model)
    {
        if (id != model.StudentId)
        {
            return BadRequest("Invalid student id.");
        }
        
        try
        {
            await _studentRepository.AddCourseAsync(model);

            if (await _studentRepository.SaveAsync())
            {
                return Ok("Enrollment successful.");
            }

            return StatusCode(500, "Enrollment failed.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPatch("{id}/disenroll")]
    public async Task<ActionResult> DisenrollStudent(string id, StudentCourseViewModel model)
    {
        if (id != model.StudentId)
        {
            return BadRequest("Invalid student id.");
        }
        
        try
        {
            await _studentRepository.RemoveCourseAsync(model);

            if (await _studentRepository.SaveAsync())
            {
                return Ok("Disenrollment successful.");
            }

            return StatusCode(500, "Disenrollment failed.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}