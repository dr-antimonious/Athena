using System.Net;
using System.Net.Http.Headers;
using Artemis.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace ShootingWebsite.Pages
{
    public class Interface : PageModel
    {
        public List<MatchOutputDto> matches;

        public UserDto user;

        private readonly IHttpClientFactory _httpClientFactory;

        public async Task<IActionResult?> OnGet()
        {
            if (Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
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

                if (response.StatusCode.Equals(HttpStatusCode.NotFound))
                {
                    TempData["AlertDanger"] = "User not found.";
                    Response.Cookies.Delete("Bearer");
                    return RedirectToPage("/Index");
                }

                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    TempData["AlertDanger"] = "Something went wrong. Please try again.";
                    Response.Cookies.Delete("Bearer");
                    return RedirectToPage("/Index");
                }

                user = response.Content.ReadFromJsonAsync<UserDto>().Result!;

                request = new HttpRequestMessage(HttpMethod.Get,
                    "http://172.19.0.3:80/artemis/data/match/get/by-user");
                request.Headers.Authorization = header;
                response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                {
                    TempData["AlertDanger"] = "You are not logged in.";
                    Response.Cookies.Delete("Bearer");
                    return RedirectToPage("/Index");
                }

                if (response.StatusCode.Equals(HttpStatusCode.NotFound))
                {
                    TempData["AlertSuccess"] = "Record your first match.";
                    return RedirectToPage("/AddMatch");
                }

                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    TempData["AlertDanger"] = "Something went wrong. Please try again.";
                    Response.Cookies.Delete("Bearer");
                    return RedirectToPage("/Index");
                }

                matches = response.Content.ReadFromJsonAsync<List<MatchOutputDto>>().Result!
                    .OrderByDescending(x => x.StartTimestamp.TimeStamp.Date).ToList();
                return Page();
            }

            TempData["AlertDanger"] = "You are not logged in.";
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult?> OnPost()
        {
            if (!Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                return RedirectToPage("/Index");
            }

            List<string?> ids = Request.Form["select"].ToArray().ToList();
            ids.RemoveAll(x => x.IsNullOrEmpty());
            List<string> cleanIds = new(ids!);

            if (cleanIds.IsNullOrEmpty())
            {
                TempData["AlertDanger"] = "No matches selected, no matches deleted.";
                return Page();
            }

            var request = new HttpRequestMessage(HttpMethod.Delete,
                "http://172.19.0.3:80/artemis/data/match/multi-delete");
            var client = _httpClientFactory.CreateClient();
            var header = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Authorization = header;
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                TempData["AlertSuccess"] = "Selected matches were deleted.";
                return RedirectToPage("/Interface");
            }

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                return RedirectToPage("/Index");
            }

            TempData["AlertDanger"] = "Something went wrong. Please try again.";
            return RedirectToPage("/Index");
        }

        public Interface(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            matches = new();
        }
    }
}
