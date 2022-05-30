using Microsoft.AspNetCore.Mvc;
using StudentApp.Models;
using StudentApp.ViewModels;

namespace StudentApp.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly AuthServiceModel _authServiceModel;

    public AuthController(IConfiguration config)
    {
        _authServiceModel = new AuthServiceModel(config);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserViewModel model)
    {
        try
        {
            var success = await _authServiceModel.RegisterUserAsync(model);

            if (success)
            {
                return RedirectToAction("Home", "Index", null);
            }

            return View("Error");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Error");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        try
        {
            var success = await _authServiceModel.LoginAsync(model);

            if (success)
            {
                return RedirectToAction("Home", "Index", null);
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