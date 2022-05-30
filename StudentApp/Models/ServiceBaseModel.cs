﻿using System.Text.Json;
using NuGet.Packaging.Core;

namespace StudentApp.Models;

public abstract class ServiceBaseModel
{
    protected string BaseUrl { get; }
    protected IConfiguration Config { get; }
    protected JsonSerializerOptions Options { get; }

    protected ServiceBaseModel(IConfiguration config, string modelUrl)
    {
        Config = config;
        BaseUrl = $"{Config.GetValue<string>("baseUrl")}/{modelUrl}";
        Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    protected async Task<List<TViewModel>> GetItemsOfTypeAsync<TViewModel>()
    {
        var response = await HttpGetResponseMessageAsync($"{BaseUrl}/list");

        var items = await response.Content.ReadFromJsonAsync<List<TViewModel>>();

        return items ?? new List<TViewModel>();
    }

    protected virtual async Task<TViewModel> GetByIdAsync<TViewModel>(string id)
    {
        var response = await HttpGetResponseMessageAsync($"{BaseUrl}/{id}");

        var item = await response.Content.ReadFromJsonAsync<TViewModel>();

        return item!;
    }

    protected async Task<HttpResponseMessage> HttpGetResponseMessageAsync(string url)
    {
        var client = new HttpClient();
        var response = await client.GetAsync(url);

        return response.IsSuccessStatusCode
            ? response
            : throw new Exception($"Something went wrong while fetching data.");
    }

    protected async Task<HttpResponseMessage> HttpPostResponseMessageAsync<TPostModel>(string url, TPostModel model)
    {
        var client = new HttpClient();
        return await client.PostAsJsonAsync(url, model);
    }
}