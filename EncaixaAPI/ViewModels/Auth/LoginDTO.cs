using System.ComponentModel.DataAnnotations;

namespace EncaixaAPI.ViewModels.Auth
{
    public class LoginDto
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
