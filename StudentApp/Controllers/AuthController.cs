using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StudentApp.Helpers;
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

    [HttpGet("register")]
    public ActionResult Register() => View();


    [HttpGet("login")]
    public ActionResult Login() => View();

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserViewModel model)
    {
        try
        {
            var registerResponse = await _authServiceModel
                .RegisterUserAsync(model);

            if (registerResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("", "", null);
            }

            return View("Register", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Register", model);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        try
        {
            var loginResponse = await _authServiceModel
                .LoginAsync(model);


            if (Request.Cookies.TryGetValue("token", out _))
            {
                var token = await this.GetJsonPropertyFromHttpResponseMessage(loginResponse, "token");
                Response.Cookies.Append("token", token);
            }
            
            if (loginResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("", "", null);
            }

            return View("Login", model);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return View("Login", model);
        }
    }
}