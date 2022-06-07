using AdminApp.ViewModels;
using AdminApp.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace AdminApp.Models;

public class AuthServiceModel : ServiceBaseModel
{
    public AuthServiceModel(IConfiguration config) 
        : base(config, "auth") { }

    public async Task<HttpResponseMessage> RegisterUserAsync(RegisterUserViewModel model)
    {
        var response = await OnPostAsync($"{BaseUrl}/register", model);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return response;
    }

    public async Task<HttpResponseMessage> EditUserAsync(string id, PatchUserViewModel model)
    {
        var response = await OnPatchAsync($"{id}", model);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return response;
    }

    public async Task<HttpResponseMessage> DeleteUserAsync(string id)
    {
        var response = await OnDeleteAsync(id);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return response;
    }
}