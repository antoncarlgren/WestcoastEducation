using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace AdminApp.Models;

public abstract class ServiceBaseModel
{
    protected string BaseUrl { get; }
    private IConfiguration _config { get; }
    private JsonSerializerOptions _options { get; }

    protected ServiceBaseModel(IConfiguration config, string modelUrl)
    {
        _config = config;
        BaseUrl = $"{_config.GetValue<string>("baseUrl")}/{modelUrl}";
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    protected async Task<List<TViewModel>> GetItemsOfTypeAsync<TViewModel>()
    {
        var response = await OnGetAsync($"{BaseUrl}/list");

        var items = await response.Content.ReadFromJsonAsync<List<TViewModel>>();

        return items ?? new List<TViewModel>();
    }

    protected virtual async Task<TViewModel> GetByIdAsync<TViewModel>(string id)
    {
        var response = await OnGetAsync($"{BaseUrl}/{id}");

        var item = await response.Content.ReadFromJsonAsync<TViewModel>();

        return item!;
    }

    protected async Task<HttpResponseMessage> PostItemAsync<TPostModel>(TPostModel model)
    {
        var response = await OnPostAsync(BaseUrl, model);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return response;
    }
    
    protected async Task<HttpResponseMessage> OnGetAsync(string url)
    {
        using var client = new HttpClient();
        
        var response = await client.GetAsync(url);

        return response.IsSuccessStatusCode
            ? response
            : throw new Exception("Something went wrong while fetching data.");
    }
    
    protected async Task<HttpResponseMessage> 
        OnPostAsync<TPostModel>(string url, TPostModel model)
    {
        using var client = new HttpClient();

        var response = await client.PostAsJsonAsync(url, model);
        
        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return response;
    }

    protected async Task<HttpResponseMessage> 
        OnPatchAsync<TPostModel>(string urlSuffix, TPostModel model)
    {
        using var client = new HttpClient();
        
        var content = new StringContent(
            JsonSerializer.Serialize(model), 
            Encoding.UTF8, 
            "application/json");
        
        Console.WriteLine($"{BaseUrl}/{urlSuffix}");
        
        var response = await client.PatchAsync($"{BaseUrl}/{urlSuffix}", content);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return response;
    }

    protected async Task<HttpResponseMessage> OnDeleteAsync(string id)
    {
        using var client = new HttpClient();

        var response = await client.DeleteAsync($"{BaseUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var reason = await response.Content.ReadAsStringAsync();
        Console.WriteLine(reason);
        return response;
    }
}