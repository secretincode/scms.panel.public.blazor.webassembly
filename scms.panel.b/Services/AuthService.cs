using scms.panel.b.Abstractions;
using scms.panel.b.Enums;
using scms.panel.b.Models.Users;
using scms.panel.b.Services.AuthStateProviders;
using System.Security.Claims;

namespace scms.panel.b.Services;

public class AuthService : IAuthService
{
    readonly ILocalStorageService _localStorageService;
    readonly IJwtHttpService _httpService;
    readonly IConfiguration _configuration;
    readonly JwtAuthenticationStateProvider _authenticationStateProvider;

    public AuthService(ILocalStorageService localStorageService, IJwtHttpService httpService, IConfiguration configuration, JwtAuthenticationStateProvider authenticationStateProvider)
    {
        _localStorageService = localStorageService;
        _httpService = httpService;
        _configuration = configuration;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public User User { get; private set; }

    public async Task Initialize()
    {
        User = await _localStorageService.GetItem<User>(_configuration["LocalStorage:UserPath"]);
    }

    public async Task Login(string userNameOrEmail, string password)
    {
        User = await _httpService.Post<User>("https://localhost:7035/api/User/Login", new { userNameOrEmail, password });
        //User = await _httpService.Post<User>("User/Login", new { userNameOrEmail, password });
        if (User.AccessToken == null)
        {
            throw new NotImplementedException("Not login");
        }
        await _localStorageService.SetItem(_configuration["LocalStorage:UserPath"], User);
        await StateProviderCheckAsync();
    }

    public async Task ExternalLoginAsync(ExternalLoginType type, string codeOrAuthToken, string state)
    {
        string apiUrl = $"https://localhost:7035/api/User/{type.ToString()}Login";
        User = await _httpService.Post<User>(apiUrl, new { codeOrAuthToken, state });
        if (!User.Success || User.AccessToken == null)
        {
            throw new NotImplementedException(User.Message);
        }
        await _localStorageService.SetItem(_configuration["LocalStorage:UserPath"], User);
        await StateProviderCheckAsync();
    }
        
    public async Task LogoutAsync()
    {
        User = null;
        await _localStorageService.RemoveItem(_configuration["LocalStorage:UserPath"]);
        await StateProviderCheckAsync();
    }

    public async Task StateProviderCheckAsync()
    {
        await _authenticationStateProvider.StateChanged();
    }

    public AuthenticationState? AuthState()
    {
        return _authenticationStateProvider.AuthState;
    }

    public ClaimsPrincipal? AuthClaimsPrincipal()
    {
        return _authenticationStateProvider.ClaimsPrincipal;
    }

    
}
