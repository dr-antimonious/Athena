using System.ComponentModel.DataAnnotations;

namespace Artemis.Contracts.DTOs
{
    public class UserRequestBaseDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = null!;

        public string? AdditionalNames { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required"),
         RegularExpression(@"^([FMXfmx]?){1,1}$",
             ErrorMessage = "Gender must be 'f', 'm', or 'x'.")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Email is required"), EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required"), Phone]
        public string PhoneNumber { get; set; } = null!;

        public UserRequestBaseDto()
        {
        }

        public UserRequestBaseDto(UserDto dto)
        {
            this.FirstName = dto.FirstName;
            this.AdditionalNames = dto.AdditionalNames;
            this.LastName = dto.LastName;
            this.DateOfBirth = dto.DateOfBirth;
            this.Gender = dto.Gender;
            this.Email = dto.Email;
            this.PhoneNumber = dto.PhoneNumber;
        }
    }
}
