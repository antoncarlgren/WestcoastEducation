using StudentApp.ViewModels;

namespace StudentApp.Models;

public class AuthServiceModel : ServiceBaseModel
{
    public AuthServiceModel(IConfiguration config) 
        : base(config, "auth") { }

    public async Task<(bool Success, HttpResponseMessage Message)> RegisterUserAsync(RegisterUserViewModel model)
    {
        var response = await HttpPostResponseMessageAsync($"{BaseUrl}/register", model);

        if (response.IsSuccessStatusCode)
        {
            return (true, response);
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return (false, response);
    }

    public async Task<(bool Success, HttpResponseMessage Message)> LoginAsync(LoginViewModel model)
    {
        var response = await HttpPostResponseMessageAsync($"{BaseUrl}/login", model);

        if (response.IsSuccessStatusCode)
        {
            return (true, response);
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return (false, response);
    }
}