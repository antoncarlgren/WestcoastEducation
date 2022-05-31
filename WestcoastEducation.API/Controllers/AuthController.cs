using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Authorization;
using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Controllers;

[ApiController]
[Authorize]
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
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserViewModel>> Register(RegisterUserViewModel model)
    {
        var user = new ApplicationUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            Id = Guid.NewGuid().ToString(),
            Email = model.Email!.ToLower(),
            UserName = model.Email.ToLower()
        };

        // Set id of registeruserviewmodel to the generated id,
        // so that it maps properly to student/teacher entities
        model.Id = user.Id;
        
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
                await _userManager.AddClaimAsync(user, new("Teacher", "false"));
                
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

    [AllowAnonymous]
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
            Expires = DateTime.Now.AddDays(7),
            Token = await CreateJwtToken(user)
        };

        return Ok(userData);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateUser(string id, PatchApplicationUserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound($"Could not find user with id {id}.");
        }

        try
        {
            var claims = await _userManager.GetClaimsAsync(user);

            if (claims.Any(c => c.Type == "Teacher" && c.Value == "true"))
            {
                await _teacherRepository.UpdateAsync(id, model);
            }
            else
            {
                await _studentRepository.UpdateAsync(id, model);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(string id, RegisterUserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound($"Could not find user with id {id}.");
        }

        var isSignedIn = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (!isSignedIn.Succeeded)
        {
            return Unauthorized();
        }
        
        try
        {
            var claims = await _userManager.GetClaimsAsync(user);

            if (claims.Any(c => c.Type == "Teacher" && c.Value == "true"))
            {
                await _teacherRepository.UpdateAsync(id, model);
            }
            else
            {
                await _studentRepository.UpdateAsync(id, model);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPatch("{id}/password")]
    public async Task<ActionResult> UpdatePassword(string id, UpdatePasswordViewModel model)
    {
        if (string.IsNullOrEmpty(model.Password)
            || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("Password cannot be null, empty, or whitespace.");
        }
        
        
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound($"User with id {id} could not be found.");
        }

        var removeResult = await _userManager.RemovePasswordAsync(user);
        var addResult = await _userManager.AddPasswordAsync(user, model.Password);

        if (!removeResult.Succeeded || !addResult.Succeeded)
        {
            return Unauthorized("Could not change password.");
        }
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound($"Could not find user with id {id}");
        }

        try
        {
            var claims = await _userManager.GetClaimsAsync(user);

            if (claims.Any(c => c.Type == "Teacher" && c.Value == "true"))
            {
                var teacherId = await _teacherRepository.GetIdByApplicationUserIdAsync(user.Id);

                await _teacherRepository.DeleteAsync(teacherId);

                if (!await _teacherRepository.SaveAsync())
                {
                    return StatusCode(500, $"Could not delete {nameof(Teacher).ToLower()}.");
                }
            }
            else
            {
                var studentId = await _studentRepository.GetIdByApplicationUserIdAsync(user.Id);

                await _studentRepository.DeleteAsync(studentId);

                if (!await _studentRepository.SaveAsync())
                {
                    return StatusCode(500, $"Could not delete {nameof(Student).ToLower()}.");
                }
            }

            await _userManager.DeleteAsync(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        
        return NoContent();
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