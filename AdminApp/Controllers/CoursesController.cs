using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers;

public class CoursesController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}