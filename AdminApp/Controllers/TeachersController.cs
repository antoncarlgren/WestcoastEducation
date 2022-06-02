using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers;

public class TeachersController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}