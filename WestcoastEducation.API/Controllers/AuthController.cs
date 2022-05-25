using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Authorization;

namespace WestcoastEducation.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : Controller
{
    private readonly IConfiguration _config;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(        
        IConfiguration config,
        IStudentRepository studentRepository,
        ITeacherRepository teacherRepository,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _config = config;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<UserViewModel>> Register(RegisterUserViewModel model)
    {
        var user = new ApplicationUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email!.ToLower(),
            UserName = model.Email.ToLower()
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var identityError in result.Errors)
            {
                ModelState.AddModelError("Student registration", identityError.Description);
            }

            return StatusCode(500, ModelState);
        }

        try
        {
            await _userManager.AddClaimsAsync(user, new Claim[]
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email)
            });
            
            if (model.IsTeacher)
            {
                await _userManager.AddClaimAsync(user, new("Teacher", "true"));
                
                await _teacherRepository.AddAsync(model);

                if (!await _teacherRepository.SaveAsync())
                {
                    return StatusCode(500, $"Could not save {nameof(Teacher).ToLower()} to database.");
                }
            }
            else
            {
                await _studentRepository.AddAsync(model);

                if (!await _studentRepository.SaveAsync())
                {
                    return StatusCode(500, $"Could not save {nameof(Student).ToLower()} to database.");
                }
            }
            
            var userData = new UserViewModel
            {
                UserName = user.UserName,
                Token = await CreateJwtToken(user)
            };

            return Ok(userData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserViewModel>> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user is null)
        {
            return Unauthorized("Invalid user name.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized();
        }
        
        var userData = new UserViewModel
        {
            UserName = model.UserName,
            Token = await CreateJwtToken(user)
        };

        return Ok(userData);
    }

    private async Task<string> CreateJwtToken(ApplicationUser user)
    {
        var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("apiKey"));
        var userClaims = (await _userManager.GetClaimsAsync(user)).ToList();

        var jwt = new JwtSecurityToken(
            claims: userClaims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
                )     
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}