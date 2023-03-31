namespace scms.panel.b.Models.Users;

public class User : BaseResponse
{
    public string? AccessToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    public string? RefreshToken { get; set; }
}
