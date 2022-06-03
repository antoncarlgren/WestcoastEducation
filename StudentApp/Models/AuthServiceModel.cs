using StudentApp.ViewModels;

namespace StudentApp.Models;

public class AuthServiceModel : ServiceBaseModel
{
    public AuthServiceModel(IConfiguration config)
        : base(config, "auth")
    {
    }

    public async Task<HttpResponseMessage> RegisterUserAsync(RegisterUserViewModel model)
    {
        var response = await HttpPostResponseMessageAsync($"{BaseUrl}/register", model);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return response;
    }

    public async Task<HttpResponseMessage> LoginAsync(LoginViewModel model)
    {
        var response = await HttpPostResponseMessageAsync($"{BaseUrl}/login", model);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return response;
    }
}