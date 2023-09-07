using Artemis.Contracts.DTOs;
using Artemis.Contracts.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace ShootingWebsite.Pages
{
    public class EditAccountModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public UserDto UserRequest { get; set; }

        public async Task<IActionResult?> OnGet()
        {
            if (Request.Cookies.TryGetValue("Bearer", out string? bearerToken))
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    "http://172.19.0.3:80/artemis/auth/get-user/by-id");
                var client = _httpClientFactory.CreateClient();
                var header = new AuthenticationHeaderValue("Bearer", bearerToken);
                request.Headers.Authorization = header;
                HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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

                UserRequest = response.Content.ReadFromJsonAsync<UserDto>().Result!;

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

            var request = new HttpRequestMessage(HttpMethod.Get,
                "http://172.19.0.3:80/artemis/auth/get-user/by-id");
            var client = _httpClientFactory.CreateClient();
            var header = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Authorization = header;
            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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

            UserRequest.Id = response.Content.ReadFromJsonAsync<UserDto>().Result!.Id;
            UserRequest.FirstName = Request.Form["UserRequest.FirstName"].ToString();
            UserRequest.AdditionalNames = Request.Form["UserRequest.AdditionalNames"].ToString();
            UserRequest.LastName = Request.Form["UserRequest.LastName"].ToString();
            UserRequest.DateOfBirth = DateTime.Parse(Request.Form["UserRequest.DateOfBirth"].ToString());
            UserRequest.Gender = Request.Form["UserRequest.Gender"].ToString()[0];
            UserRequest.PhoneNumber = Request.Form["UserRequest.PhoneNumber"].ToString();
            UserRequest.Email = Request.Form["UserRequest.Email"].ToString();

            request = new HttpRequestMessage(HttpMethod.Post,
                "http://172.19.0.3:80/artemis/auth/update");
            UserUpdateRequestDto updateRequest = new(UserRequest);

            var json = JsonConvert.SerializeObject(updateRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = content;
            request.Headers.Authorization = new("Bearer", bearerToken);
            response = await client.SendAsync(request);

            if (response.StatusCode.Equals(HttpStatusCode.Conflict))
            {
                TempData["AlertDanger"] = "User with entered e-mail already exists.";
                return Page();
            }

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                TempData["AlertSuccess"] = "Successfully changed account details";
                return RedirectToPage("/Interface");
            }

            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                TempData["AlertDanger"] = "User not found.";
                return Page();
            }

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                TempData["AlertDanger"] = "You are not logged in.";
                return RedirectToPage("/Index");
            }

            TempData["AlertDanger"] = "Something went wrong. Please try again.";
            return Page();
        }

        public EditAccountModel(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;
    }
}
