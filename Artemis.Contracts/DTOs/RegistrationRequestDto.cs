using System.ComponentModel.DataAnnotations;

namespace Artemis.Contracts.DTOs
{
    public class RegistrationRequestDto : UserRequestBaseDto
    {
        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; } = null!;

        public RegistrationRequestDto() : base()
        {
        }

        public RegistrationRequestDto(UserRequestBaseDto baseRequest)
            : base()
        {
            this.FirstName = baseRequest.FirstName;
            this.AdditionalNames = baseRequest.AdditionalNames;
            this.LastName = baseRequest.LastName;
            this.DateOfBirth = baseRequest.DateOfBirth;
            this.Gender = baseRequest.Gender;
            this.PhoneNumber = baseRequest.PhoneNumber;
            this.Email = baseRequest.Email;
        }
    }
}
