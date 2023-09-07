using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShootingWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public async Task<IActionResult?> OnGet()
        {
            if (Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "http://172.19.0.3:80/artemis/auth/get-user/by-id");
                var client = _httpClientFactory.CreateClient();
                request.Headers.Authorization = new("Bearer", bearerToken);
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return RedirectToPage("/Interface");
                }

                Response.Cookies.Delete("Bearer");
            }
            
            return null;
        }

        public IndexModel(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;
    }
}