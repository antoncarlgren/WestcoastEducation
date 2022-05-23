using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.API.Data.Repositories.Interfaces;

namespace WestcoastEducation.API.Controllers;

[ApiController]
[Route("api/v1/teachers")]
public class TeachersController
{
    private readonly ITeacherRepository _teacherRepository;

    public TeachersController(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }
}