using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Artemis.Contracts.DTOs;
using Newtonsoft.Json;

namespace ShootingWebsite.Pages
{
    public class Login : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [Required(ErrorMessage = ConstStrings.EmailRequired),
         EmailAddress(ErrorMessage = ConstStrings.EmailInvalidFormat)]
        public string email { get; set; }

        [Required(ErrorMessage = ConstStrings.PasswordRequired)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$",
            ErrorMessage = ConstStrings.PasswordRegEx)]
        public string password { get; set; }

        public async Task<IActionResult?> OnGet()
        {
            if (Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "http://172.19.0.3:80/artemis/auth/get-user/by-id");
                var client = _httpClientFactory.CreateClient();
                request.Headers.Authorization = new("Bearer", bearerToken);
                HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return RedirectToPage("/Interface");
                }

                Response.Cookies.Delete("Bearer");
            }

            return null;
        }

        public async Task<IActionResult?> OnPost()
        {
            email = Request.Form["email"].ToString();
            password = Request.Form["password"].ToString();

            ModelState.ClearValidationState(nameof(Login));
            if (!TryValidateModel(nameof(Login)))
            {
                return Page();
            }

            var request = new HttpRequestMessage(HttpMethod.Post,
                "http://172.19.0.3:80/artemis/auth/login");
            var client = _httpClientFactory.CreateClient();

            LoginRequestDto loginRequest = new()
            {
                Email = email,
                PasswordHash = Convert.ToHexString(
                    SHA512.HashData(
                        Encoding.Default.GetBytes(password)))
                    .ToLower()
            };

            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                Response.Cookies.Append("Bearer",
                    response.Content.ReadFromJsonAsync<TokenDto>().Result!.Token);
                TempData["AlertSuccess"] = "Logged in succesfully.";
                return RedirectToPage("/Interface");
            }

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                TempData["AlertDanger"] = "Incorrect password";
                return Page();
            }

            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                TempData["AlertDanger"] = "User with entered e-mail does not exist";
                return Page();
            }

            TempData["AlertDanger"] = "Something went wrong. Please try again.";
            return Page();
        }

        public Login(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;
    }
}
