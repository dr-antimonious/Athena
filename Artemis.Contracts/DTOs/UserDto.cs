namespace Artemis.Contracts.DTOs
{
    public class UserDto : UserUpdateRequestDto
    {
        public string FullName => $"{FirstName} {AdditionalNames} {LastName}";

        public UserDto()
        {
        }

        public UserDto(
            string id,
            string firstName,
            string additionalNames,
            string lastName,
            DateTime dateOfBirth,
            char gender,
            string email,
            string phoneNumber)
            : this()
        {
            this.Id = id;
            this.FirstName = firstName;
            this.AdditionalNames = additionalNames;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }
    }
}
