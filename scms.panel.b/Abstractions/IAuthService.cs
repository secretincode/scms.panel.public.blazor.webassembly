using scms.panel.b.Enums;
using scms.panel.b.Models.Users;
using System.Security.Claims;

namespace scms.panel.b.Abstractions;

public interface IAuthService
{
    User User { get; }
    Task Login(string userNameOrEmail, string password);
    Task ExternalLoginAsync(ExternalLoginType type, string code, string state);
    Task Initialize();
    Task LogoutAsync();
    Task StateProviderCheckAsync();
    AuthenticationState? AuthState();
    ClaimsPrincipal? AuthClaimsPrincipal();
}
