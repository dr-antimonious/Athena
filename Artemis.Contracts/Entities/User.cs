using System.ComponentModel.DataAnnotations;
using Artemis.Contracts.DTOs;
using Artemis.Contracts.Entities.Matches;
using Microsoft.AspNetCore.Identity;

namespace Artemis.Contracts.Entities
{
    public class User : IdentityUser<string>
    {
        [Key]
        public override string Id { get; set; } = null!;

        [Required(ErrorMessage = "First name is required"), ProtectedPersonalData]
        public string FirstName { get; set; } = null!;

        [ProtectedPersonalData]
        public string AdditionalNames { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required"), ProtectedPersonalData]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required"), ProtectedPersonalData]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required"), ProtectedPersonalData]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Email is required"), ProtectedPersonalData, EmailAddress]
        public new string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required"), ProtectedPersonalData, Phone]
        public new string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Password is required"), ProtectedPersonalData]
        public new string PasswordHash { get; set; } = null!;

        public List<Match> Matches { get; set; }

        public string FullName => $"{FirstName} {AdditionalNames} {LastName}";

        public void UpdateValues(UserUpdateRequestDto updateRequest)
        {
            this.FirstName = updateRequest.FirstName;
            this.AdditionalNames = updateRequest.AdditionalNames;
            this.LastName = updateRequest.LastName;
            this.DateOfBirth = updateRequest.DateOfBirth;
            this.Gender = updateRequest.Gender;
            this.Email = updateRequest.Email;
            this.PhoneNumber = updateRequest.PhoneNumber;
        }

        public UserDto CreateDto()
            => new(Id,
                FirstName,
                AdditionalNames,
                LastName,
                DateOfBirth,
                Gender,
                Email,
                PhoneNumber);

        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Matches = new List<Match>();
        }

        public User(
            string firstName,
            string additionalNames,
            string lastName,
            DateTime dateOfBirth,
            char gender,
            string email,
            string phoneNumber,
            string passwordHash)
            : this()
        {
            this.FirstName = firstName;
            this.AdditionalNames = additionalNames;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.PasswordHash = passwordHash;
        }

        public User(RegistrationRequestDto registrationRequest)
            : this(
                registrationRequest.FirstName,
                registrationRequest.AdditionalNames,
                registrationRequest.LastName,
                registrationRequest.DateOfBirth,
                registrationRequest.Gender,
                registrationRequest.Email,
                registrationRequest.PhoneNumber,
                registrationRequest.PasswordHash)
        {
        }

        public User(
            string id,
            string firstName,
            string additionalNames,
            string lastName,
            DateTime dateOfBirth,
            char gender,
            string email,
            string phoneNumber,
            string passwordHash,
            List<Match> matches)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.AdditionalNames = additionalNames;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.PasswordHash = passwordHash;
            this.Matches = matches;
        }
    }
}
