using scms.panel.b.Abstractions.Helper;
using System.Security.Claims;
using System.Text.Json;

namespace scms.panel.b.Services.Helper;

public class JwtExtensions : IJwtExtensions
{
    public IEnumerable<Claim> ParseClaimsFormJWT(string jwt)
    {
        string payload = jwt.Split(".")[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(w => new Claim(w.Key, w.Value.ToString()));
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        return Convert.FromBase64String(base64);
    }
}
