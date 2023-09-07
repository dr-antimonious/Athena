using System.ComponentModel.DataAnnotations;

namespace Artemis.Contracts.DTOs
{
    public class UserDeleteRequestDto : LoginRequestDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public string Id { get; set; }

        public UserDeleteRequestDto() : base()
        {
        }
    }
}
