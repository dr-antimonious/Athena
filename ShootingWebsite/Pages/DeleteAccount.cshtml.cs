using Artemis.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace ShootingWebsite.Pages
{
    public class DeleteAccountModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public UserDeleteRequestDto UserDeleteRequest { get; set; }

        [Required(ErrorMessage = ConstStrings.PasswordRequired)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$",
            ErrorMessage = ConstStrings.PasswordRegEx)]
        public string Password { get; set; }

        public async Task<IActionResult?> OnGet()
        {
            if (!Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult?> OnPost()
        {
            if (!Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                return RedirectToPage("/Index");
            }

            var request = new HttpRequestMessage(HttpMethod.Get,
                "http://172.19.0.3:80/artemis/auth/get-user/by-id");
            var client = _httpClientFactory.CreateClient();
            var header = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Authorization = header;
            HttpResponseMessage response = await client.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead);

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                Response.Cookies.Delete("Bearer");
                return RedirectToPage("/Index");
            }

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                TempData["AlertDanger"] = "Something went wrong. Please try again.";
                return RedirectToPage("/Interface");
            }

            UserDeleteRequest.Id = response.Content.ReadFromJsonAsync<UserDto>().Result!.Id;
            UserDeleteRequest.Email = Request.Form["UserDeleteRequest.Email"].ToString();
            Password = Request.Form["Password"].ToString();

            UserDeleteRequest.PasswordHash = Convert.ToHexString(SHA512.HashData(
                Encoding.Default.GetBytes(Password))).ToLower();

            request = new HttpRequestMessage(HttpMethod.Delete,
                "http://172.19.0.3:80/artemis/auth/delete");
            request.Headers.Authorization = header;
            response = await client.SendAsync(request);

            if (response.StatusCode.Equals(HttpStatusCode.Conflict))
            {
                TempData["AlertDanger"] = "E-mail addresses do not match.";
                return Page();
            }

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                TempData["AlertSuccess"] = "Account deleted.";
                Response.Cookies.Delete("Bearer");
                return RedirectToPage("/Index");
            }

            TempData["AlertDanger"] = "Something went wrong. Please try again.";
            return RedirectToPage("/Interface");
        }

        public DeleteAccountModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            UserDeleteRequest = new();
        }
    }
}
