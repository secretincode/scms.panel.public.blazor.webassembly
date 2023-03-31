using System.ComponentModel.DataAnnotations;

namespace scms.panel.b.Models.Users;

public class UserLogin
{
    [Required]
    public string UserNameOrEmail { get; set; }

    [Required]
    public string Password { get; set; }
}
