using Microsoft.AspNetCore.Components;
using scms.panel.b.Abstractions;
using scms.panel.b.Models.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using System.Text;

namespace scms.panel.b.Services;

public class JwtHttpService : IJwtHttpService
{
    readonly HttpClient _httpClient;
    readonly NavigationManager _navigationManager;
    readonly IConfiguration _configuration;
    readonly ILocalStorageService _localStorageService;

    public JwtHttpService(HttpClient httpClient, NavigationManager navigationManager, IConfiguration configuration, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _configuration = configuration;
        _localStorageService = localStorageService;
    }

    public async Task<T> Get<T>(string uri)
    {
        HttpRequestMessage? request = new HttpRequestMessage(HttpMethod.Get, uri);
        return await sendRequest<T>(request);
    }

    public async Task<T> Post<T>(string uri, object value)
    {
        HttpRequestMessage? request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        return await sendRequest<T>(request);
    }

    private async Task<T?> sendRequest<T>(HttpRequestMessage request)
    {
        // add jwt auth header if user is logged in and request is to the api url
        User _user = await _localStorageService.GetItem<User>(_configuration["LocalStorage:UserPath"]);
        bool isApiUrl = !request.RequestUri.IsAbsoluteUri;
        if (_user != null && _user.AccessToken != null && isApiUrl)
            request.Headers.Authorization = new AuthenticationHeaderValue(_configuration["Auth:Scheme"], _user.AccessToken);

        using HttpResponseMessage? response = await _httpClient.SendAsync(request);

        // auto logout on 401 response
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _navigationManager.NavigateTo(_configuration["Auth:LogutPath"]);
            return default;
        }

        // throw exception on error response
        if (!response.IsSuccessStatusCode)
        {
            Dictionary<string, string>? error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new Exception(error?["message"]);
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }
}
