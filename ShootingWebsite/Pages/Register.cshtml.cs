using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Artemis.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ShootingWebsite.Pages
{
    public class Register : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public UserRequestBaseDto UserRequest { get; set; }

        [Required(ErrorMessage = ConstStrings.PasswordRequired)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$",
            ErrorMessage = ConstStrings.PasswordRegEx)]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult?> OnPostAsync()
        {
            UserRequest.FirstName = Request.Form["UserRequest.FirstName"].ToString();
            UserRequest.AdditionalNames = Request.Form["UserRequest.AdditionalNames"].ToString();
            UserRequest.LastName = Request.Form["UserRequest.LastName"].ToString();
            UserRequest.DateOfBirth = DateTime.Parse(Request.Form["UserRequest.DateOfBirth"].ToString());
            UserRequest.Gender = Request.Form["UserRequest.Gender"].ToString()[0];
            UserRequest.PhoneNumber = Request.Form["UserRequest.PhoneNumber"].ToString();
            UserRequest.Email = Request.Form["UserRequest.Email"].ToString();
            Password = Request.Form["Password"].ToString();

            ModelState.ClearValidationState(nameof(Register));
            if (!TryValidateModel(nameof(Register)))
            {
                return Page();
            }

            var request = new HttpRequestMessage(HttpMethod.Post,
                "http://172.19.0.3:80/artemis/auth/register");
            var client = _httpClientFactory.CreateClient();

            RegistrationRequestDto registrationRequest = new(UserRequest)
            {
                PasswordHash = Convert.ToHexString(
                        SHA512.HashData(
                            Encoding.Default.GetBytes(Password)))
                    .ToLower()
            };

            var json = JsonConvert.SerializeObject(registrationRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode.Equals(HttpStatusCode.Conflict))
            {
                TempData["AlertDanger"] = "User with entered e-mail already exists.";
                return Page();
            }

            if (response.StatusCode.Equals(HttpStatusCode.Created))
            {
                TempData["AlertSuccess"] = "Successfully registered. Please log in.";
                return RedirectToPage("/Login");
            }

            TempData["AlertDanger"] = "Something went wrong. Please try again.";
            return Page();
        }

        public Register(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            UserRequest = new UserRequestBaseDto();
        }
    }
}
