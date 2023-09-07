using Artemis.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using Artemis.Contracts.Entities;
using Artemis.Contracts.Entities.Matches;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ShootingWebsite.Pages
{
    public class AddMatchModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory; 

        [BindProperty]
        public MatchCreateRequestDto MatchCreateRequest { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string CountryName { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string LocationName { get; set; }

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
            request.Headers.Authorization = new("Bearer", bearerToken);
            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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

            MatchCreateRequest.ShooterId = response.Content.ReadFromJsonAsync<UserDto>().Result!.Id;
            MatchCreateRequest.Type = Request.Form["MatchCreateRequest.Type"].ToString();
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
            MatchCreateRequest.StartTimestampId = response.Content.ReadFromJsonAsync<Timestamp>().Result!.Id;

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
            MatchCreateRequest.EndTimestampId = response.Content.ReadFromJsonAsync<Timestamp>().Result!.Id;

            CountryName = Request.Form["CountryName"].ToString();
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
                Name = Request.Form["CityName"].ToString(),
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
            locationRequest.Name = Request.Form["LocationName"].ToString();
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
            MatchCreateRequest.LocationId = response.Content.ReadFromJsonAsync<Location>().Result!.Id;

            MatchCreateRequest.AirTemperature =
                !Request.Form["MatchCreateRequest.AirTemperature"].ToString().IsNullOrEmpty()
                    ? double.Parse(Request.Form["MatchCreateRequest.AirTemperature"].ToString())
                    : null;
            MatchCreateRequest.AirPressure =
                !Request.Form["MatchCreateRequest.AirPressure"].ToString().IsNullOrEmpty()
                    ? double.Parse(Request.Form["MatchCreateRequest.AirPressure"].ToString())
                    : null;
            MatchCreateRequest.WindSpeed =
                !Request.Form["MatchCreateRequest.WindSpeed"].ToString().IsNullOrEmpty()
                    ? double.Parse(Request.Form["MatchCreateRequest.WindSpeed"].ToString())
                    : null;
            MatchCreateRequest.WindDirection =
                !Request.Form["MatchCreateRequest.WindDirection"].ToString().Contains('X')
                    ? Request.Form["MatchCreateRequest.WindDirection"].ToString()
                    : null;
            MatchCreateRequest.EnvironmentNotes =
                !Request.Form["MatchCreateRequest.EnvironmentNotes"].ToString().IsNullOrEmpty()
                    ? Request.Form["MatchCreateRequest.EnvironmentNotes"].ToString()
                    : null;
            MatchCreateRequest.EquipmentNotes =
                !Request.Form["MatchCreateRequest.EquipmentNotes"].ToString().IsNullOrEmpty()
                    ? Request.Form["MatchCreateRequest.EquipmentNotes"].ToString()
                    : null;
            MatchCreateRequest.ShooterNotes =
                !Request.Form["MatchCreateRequest.ShooterNotes"].ToString().IsNullOrEmpty()
                    ? Request.Form["MatchCreateRequest.ShooterNotes"].ToString()
                    : null;

            List<double> shotVals = new();
            foreach(string temp in Request.Form["ShotValue"].ToArray().ToList())
                shotVals.Add(double.Parse(temp!));

            List<Timestamp?> shotTimestamp = new();
            List<double?> shotHorizontal = new();
            List<double?> shotVertical = new();

            if (!MatchCreateRequest.Type.Equals("TS"))
            {
                foreach (string temp in Request.Form["ShotTimestamp"].ToArray().ToList())
                {
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
                }

                foreach (string temp in Request.Form["ShotHorizontalDisplacement"].ToArray().ToList())
                {
                    if (!temp.IsNullOrEmpty()) shotHorizontal.Add(double.Parse(temp!));
                    else shotHorizontal.Add(null);
                }

                foreach (string temp in Request.Form["ShotVerticalDisplacement"].ToArray().ToList())
                {
                    if (!temp.IsNullOrEmpty()) shotVertical.Add(double.Parse(temp!));
                    else shotVertical.Add(null);
                }
            }

            MatchCreateRequest.Shots = new();

            for (int i = 0; i < Match.TotalShots[MatchCreateRequest.Type]; i++)
            {
                if (!MatchCreateRequest.Type.Equals("TS"))
                {
                    MatchCreateRequest.Shots.Add(new(shotVals[i], i + 1, shotTimestamp[i], shotHorizontal[i],
                        shotVertical[i]));
                }
                else
                {
                    MatchCreateRequest.Shots.Add(new(shotVals[i], i + 1));
                }
            }

            request = new HttpRequestMessage(HttpMethod.Post,
                "http://172.19.0.3:80/artemis/data/match/add");
            request.Headers.Authorization = new("Bearer", bearerToken);
            json = JsonConvert.SerializeObject(MatchCreateRequest);
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
            
            return RedirectToPage("/Interface");
        }

        public AddMatchModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            MatchCreateRequest = new()
            {
                Shots = new()
            };
        }
    }
}
