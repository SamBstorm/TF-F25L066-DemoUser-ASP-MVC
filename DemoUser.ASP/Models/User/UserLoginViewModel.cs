using System.ComponentModel.DataAnnotations;

namespace DemoUser.ASP.Models.User;

public class UserLoginViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}