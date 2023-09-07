using System.Net;
using Artemis.Contracts.Entities.Matches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Artemis.Contracts.DTOs;

namespace ShootingWebsite.Pages
{
    public class MatchDetails : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Match Match { get; set; }

        public ITuple MatchResult { get; set; }

        public async Task<IActionResult?> OnGet([FromQuery] string matchId)
        {
            if (!Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                return RedirectToPage("/Index");
            }

            if (matchId.IsNullOrEmpty())
            {
                TempData["AlertDanger"] = "No ID provided for match view.";
                return RedirectToPage("/Interface");
            }

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"http://172.19.0.3:80/artemis/data/match/get/by-id?id={matchId}");
            var client = _httpClientFactory.CreateClient();
            var header = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Authorization = header;
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                TempData["AlertDanger"] = "Match not found.";
                return RedirectToPage("/Interface");
            }

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                Response.Cookies.Delete("Bearer");
                return RedirectToPage("/Index");
            }

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
            {
                TempData["AlertDanger"] = "Something went wrong. Please try again later.";
                return RedirectToPage("/Interface");
            }

            MatchOutputDto dto = response.Content.ReadFromJsonAsync<MatchOutputDto>().Result!;
            if (Match.ConvertMatch.TryGetValue(dto.Type,
                    out Func<MatchOutputDto, Match>? creator))
            {
                Match = creator(dto);
                Match.Shots = Match.Shots.OrderBy(x => x.Position).ToList();
                MatchResult = Match.GetMatchResult();
                return Page();
            }

            TempData["AlertDanger"] = "Something went wrong. Please try again later.";
            return RedirectToPage("/Interface");
        }

        public MatchDetails(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;
    }
}
