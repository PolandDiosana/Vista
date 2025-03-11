using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Vista_Subdivision.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Vista_Subdivision.Pages
{
    public class RegisterModel : PageModel
    {
        private const string SessionKey = "UserRegistration";

        [BindProperty]
        public IFormFile ProfileFile { get; set; }

        [BindProperty]
        public UserRegistration Input { get; set; } = new UserRegistration();

        private UserRegistration GetSessionData()
        {
            var sessionData = HttpContext.Session.GetString(SessionKey);
            return sessionData != null ? JsonConvert.DeserializeObject<UserRegistration>(sessionData) ?? new UserRegistration() : new UserRegistration();
        }

        private void SaveSessionData(UserRegistration model)
        {
            HttpContext.Session.SetString(SessionKey, JsonConvert.SerializeObject(model));
            Console.WriteLine($"🔍 Session Data After Saving: {JsonConvert.SerializeObject(model)}");
        }

        public void OnGet()
        {
            Input = GetSessionData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("🚀 OnPost() triggered in Register!");
            ModelState.Remove("ProfileFile");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState is invalid!");
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Field: {entry.Key} - Error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            var sessionData = GetSessionData() ?? new UserRegistration();

            sessionData.FirstName = Input.FirstName;
            sessionData.LastName = Input.LastName;
            sessionData.Email = Input.Email;
            sessionData.PhoneNumber = Input.PhoneNumber;
            sessionData.Username = Input.Username;
            sessionData.Password = Input.Password;

            if (ProfileFile != null && ProfileFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ProfileFile.CopyToAsync(memoryStream);
                    byte[] profileBytes = memoryStream.ToArray();
                    HttpContext.Session.Set("ProfilePending", profileBytes);
                }
                sessionData.Profile = "PENDING_UPLOAD";
                Console.WriteLine("📂 Profile file stored in session for later upload.");
            }

            SaveSessionData(sessionData);
            Console.WriteLine("✅ Data saved into session successfully!");

            return RedirectToPage("/Register2");
        }
    }
}
