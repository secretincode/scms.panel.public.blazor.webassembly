using Microsoft.AspNetCore.Components.Authorization;
using scms.panel.b.Abstractions;
using scms.panel.b.Abstractions.Helper;
using scms.panel.b.Models.Users;
using System.Security.Claims;

namespace scms.panel.b.Services.AuthStateProviders;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{

    readonly IJwtExtensions _jwtExtensions;
    readonly ILocalStorageService _localStorageService;
    readonly IConfiguration _configuration;

    public User User { get; private set; }
    public AuthenticationState AuthState { get; private set; }
    public ClaimsPrincipal ClaimsPrincipal { get; private set; }

    public JwtAuthenticationStateProvider(IJwtExtensions jwtExtensions, ILocalStorageService localStorageService, IConfiguration configuration)
    {
        _jwtExtensions = jwtExtensions;
        _localStorageService = localStorageService;
        _configuration = configuration;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        User = await _localStorageService.GetItem<User>(_configuration["LocalStorage:UserPath"]);

        ClaimsIdentity identity;
        try
        {
            if (User == null || User.AccessToken == null || DateTime.UtcNow > User.AccessTokenExpiration)
            {
                await _localStorageService.RemoveItem(_configuration["LocalStorage:UserPath"]);
                identity = new ClaimsIdentity();
            }
            else
            {
                identity = new ClaimsIdentity(_jwtExtensions.ParseClaimsFormJWT(User.AccessToken), "jwt");
            }

        }
        catch (Exception)
        {
            await _localStorageService.RemoveItem(_configuration["LocalStorage:UserPath"]);
            identity = new ClaimsIdentity();
        }

        ClaimsPrincipal = new ClaimsPrincipal(identity);
        AuthState = new AuthenticationState(ClaimsPrincipal);
        NotifyAuthenticationStateChanged(Task.FromResult(AuthState));

        return AuthState;
    }

    public async Task StateChanged()
    {
        await GetAuthenticationStateAsync();
    }
}
