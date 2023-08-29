using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ShootingWebsite.Pages
{
    public class Login : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [Required(ErrorMessage = "Email is required"), EmailAddress]
        public string email;

        [Required(ErrorMessage = "Password is required")]
        public string password;

        public async Task<IActionResult?> OnGet()
        {
            if (Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "http://api.shooting.advanc.eu/artemis/auth/get/by-id");
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

        public async Task<RedirectResult?> OnPost()
        {
            
            return null;
        }

        public Login(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;
    }
}
