using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.API.Data.Repositories.Interfaces;

namespace WestcoastEducation.API.Controllers;

[ApiController]
[Route("api/v1/students")]
public class StudentController
{
    private readonly IStudentRepository _studentRepository;

    public StudentController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
}