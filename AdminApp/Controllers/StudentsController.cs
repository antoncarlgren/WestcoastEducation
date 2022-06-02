using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers;

public class StudentsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}