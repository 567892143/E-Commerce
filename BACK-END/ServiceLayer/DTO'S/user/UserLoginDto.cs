
using System.ComponentModel.DataAnnotations;
namespace ServiceLayer.User.Dto;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email or username is required.")]
    public string email { get; set; } // Can be email or username

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}
