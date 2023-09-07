using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net;
using Artemis.Contracts.DTOs;
using Artemis.Contracts.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using Artemis.Contracts.Entities.Matches;

namespace ShootingWebsite.Pages
{
    public class EditMatch : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public MatchUpdateRequestDto MatchUpdateRequest { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public Timestamp StartTimestamp { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public Timestamp EndTimestamp { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public Country Country { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public City City { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public Location Location { get; set; }

        public async Task<IActionResult?> OnGet([FromQuery] string matchId)
        {
            if (!Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                return RedirectToPage("/Index");
            }

            if (matchId.IsNullOrEmpty())
            {
                TempData["AlertDanger"] = "No ID provided for match edit.";
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

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                MatchOutputDto dto = response.Content.ReadFromJsonAsync<MatchOutputDto>().Result!;
                MatchUpdateRequest = new(dto);
                StartTimestamp = dto.StartTimestamp;
                EndTimestamp = dto.EndTimestamp;
                Date = StartTimestamp.TimeStamp.Date;
                StartTime = StartTimestamp.TimeStamp;
                EndTime = EndTimestamp.TimeStamp;
                Location = dto.Location;
                City = Location.City;
                Country = Location.Country;
                MatchUpdateRequest.Shots = MatchUpdateRequest.Shots.OrderBy(x => x.Position).ToList();
                return Page();
            }

            TempData["AlertDanger"] = "Something went wrong. Please try again later.";
            return RedirectToPage("/Interface");
        }

        public async Task<IActionResult?> OnPost([FromQuery] string matchId)
        {
            if (!Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                return RedirectToPage("/Index");
            }

            if (matchId.IsNullOrEmpty())
            {
                TempData["AlertDanger"] = "No ID provided for match edit.";
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

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                MatchOutputDto dto = response.Content.ReadFromJsonAsync<MatchOutputDto>().Result!;
                MatchUpdateRequest = new(dto);

                Date = DateTime.Parse(Request.Form["Date"].ToString());

                StartTime = DateTime.Parse(Request.Form["StartTime"].ToString());
                DateTime start = new(Date.Date.Year, Date.Date.Month, Date.Date.Day,
                    StartTime.Hour, StartTime.Minute, 0);
                request = new HttpRequestMessage(HttpMethod.Post,
                    "http://172.19.0.3:80/artemis/data/timestamp/add");
                request.Headers.Authorization = new("Bearer", bearerToken);
                var json = JsonConvert.SerializeObject(start);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = content;
                response = await client.SendAsync(request);
                if (!response.StatusCode.Equals(HttpStatusCode.Conflict)
                    && !response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    TempData["AlertDanger"] = "Something went wrong. Please try again.";
                    return Page();
                }
                MatchUpdateRequest.StartTimestampId = response.Content.ReadFromJsonAsync<Timestamp>().Result!.Id;

                EndTime = DateTime.Parse(Request.Form["EndTime"].ToString());
                DateTime end = new(Date.Date.Year, Date.Date.Month, Date.Date.Day,
                    EndTime.Hour, EndTime.Minute, 0);
                request = new HttpRequestMessage(HttpMethod.Post,
                    "http://172.19.0.3:80/artemis/data/timestamp/add");
                request.Headers.Authorization = new("Bearer", bearerToken);
                json = JsonConvert.SerializeObject(end);
                content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = content;
                response = await client.SendAsync(request);
                if (!response.StatusCode.Equals(HttpStatusCode.Conflict)
                    && !response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    TempData["AlertDanger"] = "Something went wrong. Please try again.";
                    return Page();
                }
                MatchUpdateRequest.EndTimestampId = response.Content.ReadFromJsonAsync<Timestamp>().Result!.Id;

                string CountryName = Request.Form["Country.Name"].ToString();
                request = new HttpRequestMessage(HttpMethod.Post,
                    $"http://172.19.0.3:80/artemis/data/country/add");
                request.Headers.Authorization = new("Bearer", bearerToken);
                json = JsonConvert.SerializeObject(CountryName);
                content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = content;
                response = await client.SendAsync(request);
                if (!response.StatusCode.Equals(HttpStatusCode.Conflict)
                    && !response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    TempData["AlertDanger"] = "Something went wrong. Please try again.";
                    return Page();
                }
                Country country = response.Content.ReadFromJsonAsync<Country>().Result!;

                CityCreateRequestDto cityRequest = new()
                {
                    Name = Request.Form["City.Name"].ToString(),
                    Id = country.Id
                };
                request = new HttpRequestMessage(HttpMethod.Post,
                    $"http://172.19.0.3:80/artemis/data/city/add");
                request.Headers.Authorization = new("Bearer", bearerToken);
                json = JsonConvert.SerializeObject(cityRequest);
                content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = content;
                response = await client.SendAsync(request);
                if (!response.StatusCode.Equals(HttpStatusCode.Conflict)
                    && !response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    TempData["AlertDanger"] = "Something went wrong. Please try again.";
                    return Page();
                }
                City city = response.Content.ReadFromJsonAsync<City>().Result!;

                LocationCreateRequestDto locationRequest = new();
                locationRequest.CityId = city.Id;
                locationRequest.CountryId = country.Id;
                locationRequest.Name = Request.Form["Location.Name"].ToString();
                request = new HttpRequestMessage(HttpMethod.Post,
                    $"http://172.19.0.3:80/artemis/data/location/add");
                request.Headers.Authorization = new("Bearer", bearerToken);
                json = JsonConvert.SerializeObject(locationRequest);
                content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = content;
                response = await client.SendAsync(request);
                if (!response.StatusCode.Equals(HttpStatusCode.Conflict)
                    && !response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    TempData["AlertDanger"] = "Something went wrong. Please try again.";
                    return Page();
                }
                MatchUpdateRequest.LocationId = response.Content.ReadFromJsonAsync<Location>().Result!.Id;

                MatchUpdateRequest.AirTemperature =
                    !Request.Form["MatchUpdateRequest.AirTemperature"].ToString().IsNullOrEmpty()
                        ? double.Parse(Request.Form["MatchUpdateRequest.AirTemperature"].ToString())
                        : null;
                MatchUpdateRequest.AirPressure =
                    !Request.Form["MatchUpdateRequest.AirPressure"].ToString().IsNullOrEmpty()
                        ? double.Parse(Request.Form["MatchUpdateRequest.AirPressure"].ToString())
                        : null;
                MatchUpdateRequest.WindSpeed =
                    !Request.Form["MatchUpdateRequest.WindSpeed"].ToString().IsNullOrEmpty()
                        ? double.Parse(Request.Form["MatchUpdateRequest.WindSpeed"].ToString())
                        : null;
                MatchUpdateRequest.WindDirection =
                    !Request.Form["MatchUpdateRequest.WindDirection"].ToString().Contains('X')
                        ? Request.Form["MatchUpdateRequest.WindDirection"].ToString()
                        : null;
                MatchUpdateRequest.EnvironmentNotes =
                    !Request.Form["MatchUpdateRequest.EnvironmentNotes"].ToString().IsNullOrEmpty()
                        ? Request.Form["MatchUpdateRequest.EnvironmentNotes"].ToString()
                        : null;
                MatchUpdateRequest.EquipmentNotes =
                    !Request.Form["MatchUpdateRequest.EquipmentNotes"].ToString().IsNullOrEmpty()
                        ? Request.Form["MatchUpdateRequest.EquipmentNotes"].ToString()
                        : null;
                MatchUpdateRequest.ShooterNotes =
                    !Request.Form["MatchUpdateRequest.ShooterNotes"].ToString().IsNullOrEmpty()
                        ? Request.Form["MatchUpdateRequest.ShooterNotes"].ToString()
                        : null;

                List<double> shotVals = new();
                for (int i = 0; i < MatchUpdateRequest.Shots.Count; i++)
                    shotVals.Add(double.Parse(Request.Form[$"MatchUpdateRequest.Shots[{i}].Value"].ToString()));

                List<Timestamp?> shotTimestamp = new();
                List<double?> shotHorizontal = new();
                List<double?> shotVertical = new();

                if (!MatchUpdateRequest.Type.Equals("TS"))
                {
                    for (int i = 0; i < MatchUpdateRequest.Shots.Count; i++)
                    {
                        string temp = Request.Form[$"MatchUpdateRequest.Shots[{i}].Timestamp"].ToString();

                        if (!temp.IsNullOrEmpty())
                        {
                            DateTime stamp = DateTime.Parse(temp!);
                            request = new HttpRequestMessage(HttpMethod.Post,
                                "http://172.19.0.3:80/artemis/data/timestamp/add");
                            request.Headers.Authorization = new("Bearer", bearerToken);
                            json = JsonConvert.SerializeObject(stamp);
                            content = new StringContent(json, Encoding.UTF8, "application/json");
                            request.Content = content;
                            response = await client.SendAsync(request);
                            if (!response.StatusCode.Equals(HttpStatusCode.Conflict)
                                && !response.StatusCode.Equals(HttpStatusCode.Created))
                            {
                                TempData["AlertDanger"] = "Something went wrong. Please try again.";
                                return Page();
                            }
                            shotTimestamp.Add(response.Content.ReadFromJsonAsync<Timestamp>().Result);
                        }
                        else shotTimestamp.Add(null);

                        temp = Request.Form[$"MatchUpdateRequest.Shots[{i}].HorizontalDisplacement"].ToString();

                        if (!temp.IsNullOrEmpty()) shotHorizontal.Add(double.Parse(temp!));
                        else shotHorizontal.Add(null);

                        temp = Request.Form[$"MatchUpdateRequest.Shots[{i}].VerticalDisplacement"].ToString();

                        if (!temp.IsNullOrEmpty()) shotVertical.Add(double.Parse(temp!));
                        else shotVertical.Add(null);
                    }
                }

                for (int i = 0; i < MatchUpdateRequest.Shots.Count; i++)
                {
                    MatchUpdateRequest.Shots[i].Value = shotVals[i];

                    if (!MatchUpdateRequest.Type.Equals("TS"))
                    {
                        MatchUpdateRequest.Shots[i].Timestamp = shotTimestamp[i];
                        MatchUpdateRequest.Shots[i].HorizontalDisplacement = shotHorizontal[i];
                        MatchUpdateRequest.Shots[i].VerticalDisplacement = shotVertical[i];
                    }
                }

                request = new HttpRequestMessage(HttpMethod.Post,
                    "http://172.19.0.3:80/artemis/data/match/update");
                request.Headers.Authorization = new("Bearer", bearerToken);
                json = JsonConvert.SerializeObject(MatchUpdateRequest);
                content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = content;
                response = await client.SendAsync(request);
                if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                {
                    TempData["AlertDanger"] = "You are not logged in.";
                    Response.Cookies.Delete("Bearer");
                    return RedirectToPage("/Index");
                }

                if (!response.StatusCode.Equals(HttpStatusCode.Created))
                {
                    TempData["AlertDanger"] = "Something went wrong. Please try again.";
                    return Page();
                }

                TempData["AlertSuccess"] = "Changes applied.";
                return RedirectToPage("/Interface");
            }

            TempData["AlertDanger"] = "Something went wrong. Please try again later.";
            return RedirectToPage("/Interface");
        }

        public EditMatch(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
