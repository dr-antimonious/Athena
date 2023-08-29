using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ShootingWebsite.Pages
{
    public class Interface : PageModel
    {
        public List<Dictionary<String, String>> matchList = new List<Dictionary<string, string>>();
        public async Task<RedirectResult?> OnGet()
        {
            bool valid = false;
            DocumentSnapshot? user = null;
            FirestoreDb db = FirestoreDb.Create("shootingdiary-orwima");

            if (Request.Cookies.TryGetValue("userId", out String? userId))
            {
                if (String.IsNullOrWhiteSpace(userId))
                {
                    TempData["AlertDanger"] = "You are not logged in. Please log in or register.";
                    return Redirect("/Index");
                }

                CollectionReference collection = db.Collection("users");
                IAsyncEnumerable<DocumentReference> documentRefs =
                    collection.ListDocumentsAsync();

                await foreach (DocumentReference document in documentRefs)
                {
                    DocumentSnapshot temp = await document.GetSnapshotAsync();
                    if (temp.Id == userId)
                    {
                        user = temp;
                        valid = true;
                        break;
                    }
                }
            }

            else
            {
                TempData["AlertDanger"] = "You are not logged in. Please log in or register.";
                return Redirect("/Index");
            }

            if (!valid)
            {
                TempData["AlertDanger"] = "Incorrect userId found in cookies. Please login or register.";
                return Redirect("/Logout");
            }

            String? username = null;
            user?.TryGetValue("username", out username);
            if (username != null)
                TempData["username"] = username;

            const string affirmationsUri = "https://www.affirmations.dev";
            HttpClient client = new HttpClient();
            string? affirmation;

            try
            {
                affirmation = client.GetStringAsync(affirmationsUri).Result;
            }
            catch (Exception ex)
            {
                affirmation = null;
            }

            if (affirmation != null)
                affirmation = JsonConvert.DeserializeObject<Dictionary<String, String>>(affirmation)?["affirmation"];

            if (affirmation != null)
                TempData["affirmation"] = affirmation;

            CollectionReference matchesRef = db.Collection("matches");
            IAsyncEnumerable<DocumentReference> matchRefs =
                matchesRef.ListDocumentsAsync();

            await foreach (DocumentReference document in matchRefs)
            {
                DocumentSnapshot temp = await document.GetSnapshotAsync();
                if (temp.TryGetValue("userId", out String matchUser))
                {
                    if (user?.Id == matchUser)
                    {
                        temp.TryGetValue("Date", out String date);
                        temp.TryGetValue("StartTime", out String startTime);
                        temp.TryGetValue("EndTime", out String endTime);
                        temp.TryGetValue("Location", out String location);
                        temp.TryGetValue("Result", out int result);
                        temp.TryGetValue("Inner10s", out int inner10s);
                        matchList.Add(new Dictionary<string, string>
                        {
                            {"Date", date},
                            {"StartTime", startTime},
                            {"EndTime", endTime},
                            {"Location", location},
                            {"Result", result.ToString()},
                            {"Inner10s", inner10s.ToString()},
                            {"id", temp.Id}
                        });
                    }
                }
            }

            return null;
        }
    }
}
