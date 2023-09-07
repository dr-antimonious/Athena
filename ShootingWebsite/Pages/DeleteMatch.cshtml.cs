using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;

namespace ShootingWebsite.Pages
{
    public class DeleteMatch : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public async Task<IActionResult?> OnGet([FromQuery] string matchId)
        {
            if (!Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                return RedirectToPage("/Index");
            }

            if (matchId.IsNullOrEmpty())
            {
                TempData["AlertDanger"] = "No ID provided for match deletion.";
                return RedirectToPage("/Interface");
            }

            var request = new HttpRequestMessage(HttpMethod.Delete,
                $"http://172.19.0.3:80/artemis/data/match/delete?id={matchId}");
            var client = _httpClientFactory.CreateClient();
            var header = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Authorization = header;
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                TempData["AlertSuccess"] = "Match deleted successfully.";
                return RedirectToPage("/Interface");
            }

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                Response.Cookies.Delete("Bearer");
                return RedirectToPage("/Index");
            }

            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                TempData["AlertDanger"] = "Match selected for deletion was not found.";
                return RedirectToPage("/Interface");
            }

            TempData["AlertDanger"] = "Something went wrong. Please try again.";
            return RedirectToPage("/Interface");
        }

        public DeleteMatch(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;
    }
}
