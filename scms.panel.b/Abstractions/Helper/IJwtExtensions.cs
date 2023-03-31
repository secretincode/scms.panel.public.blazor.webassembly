using System.Security.Claims;

namespace scms.panel.b.Abstractions.Helper;

public interface IJwtExtensions
{
    public IEnumerable<Claim> ParseClaimsFormJWT(string jwt);
}
