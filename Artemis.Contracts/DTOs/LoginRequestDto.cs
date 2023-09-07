using System.ComponentModel.DataAnnotations;

namespace Artemis.Contracts.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email address is required"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; }
    }
}
