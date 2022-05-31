using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace StudentApp.Helpers;

public static class ControllerExtensions
{
    public static async Task<string> GetJsonPropertyFromHttpResponseMessage(
        this Controller controller, 
        HttpResponseMessage message, 
        string key)
    {
        var data = await message.Content.ReadAsStringAsync();

        var json = JObject.Parse(data);
        var jsonProperty = json?.SelectToken(key)?.Value<string>();

        return jsonProperty!;
    }
}