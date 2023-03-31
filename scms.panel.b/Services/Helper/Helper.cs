using scms.panel.b.Abstractions.Helper;
using System.Security.Cryptography;

namespace scms.panel.b.Services.Helper;

public class Helper : IHelper
{
    public string? CreateHashRandom(int iterations)
    {
        byte[] number = new byte[iterations];
        using RandomNumberGenerator random = RandomNumberGenerator.Create();
        random.GetBytes(number);
        return Convert.ToBase64String(number);
    }
}
