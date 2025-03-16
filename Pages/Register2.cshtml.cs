using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Pages
{
    public class Register2Model : PageModel
    {
        private const string SessionKey = "UserRegistration";
        private readonly ApplicationDbContext _dbContext;
        public Register2Model(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public UserRegistration Input { get; set; } = new UserRegistration();

        private UserRegistration GetSessionData()
        {
            var sessionData = HttpContext.Session.GetString(SessionKey);
            if (sessionData == null)
            {
                Console.WriteLine("No session data found!");
                return new UserRegistration();
            }
            return JsonConvert.DeserializeObject<UserRegistration>(sessionData) ?? new UserRegistration();
        }

        private void SaveSessionData(UserRegistration model)
        {
            var serializedData = JsonConvert.SerializeObject(model);
            HttpContext.Session.SetString(SessionKey, serializedData);
            Console.WriteLine($"📝 Session Data Saved: {serializedData}");
        }

        public void OnGet()
        {
            Input = GetSessionData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Field: {entry.Key} - Error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            var sessionData = GetSessionData();
            sessionData.LotNumber = Input.LotNumber;
            sessionData.BlockNumber = Input.BlockNumber;
            sessionData.HouseType = Input.HouseType;
            sessionData.SubdivisionName = Input.SubdivisionName;
            sessionData.CityMunicipality = Input.CityMunicipality;
            sessionData.StreetName = Input.StreetName;
            sessionData.Province = Input.Province;
            sessionData.Phase = Input.Phase;

            // ✅ Save Profile Image (Only if it exists in session)
            if (HttpContext.Session.TryGetValue("ProfilePending", out var profileBytes))
            {
                using (var stream = new MemoryStream(profileBytes))
                {
                    IFormFile profileFile = new FormFile(stream, 0, profileBytes.Length, "Profile", "profile.jpg");
                    string profilePath = await SaveFileAsync(profileFile);
                    sessionData.Profile = profilePath;
                    Console.WriteLine($"✅ Profile Image Saved to: {profilePath}");
                }
                HttpContext.Session.Remove("ProfilePending"); // Clear the session after saving
            }

            SaveSessionData(sessionData);

            // ✅ Insert into database
            SaveToDatabase(sessionData);

            // ✅ Clear session after saving
            HttpContext.Session.Remove(SessionKey);
            Console.WriteLine("✅ Session cleared after insert!");

            return RedirectToPage("/Login");
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{uniqueFileName}";
        }

        private void SaveToDatabase(UserRegistration sessionData)
        {
            try
            {
                // ✅ Insert into User Table
                var newUser = new User
                {
                    FirstName = sessionData.FirstName,
                    LastName = sessionData.LastName,
                    Email = sessionData.Email,
                    PhoneNumber = sessionData.PhoneNumber,
                    Username = sessionData.Username,
                    PasswordHash = sessionData.Password,
                    ProfileImage = sessionData.Profile,
                    Role = "Homeowner"
                };
                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
                Console.WriteLine("✅ User saved!");

                // ✅ Insert into Property Table
                var newProperty = new Property
                {
                    UserId = newUser.Id,
                    LotNumber = sessionData.LotNumber,
                    BlockNumber = sessionData.BlockNumber,
                    HouseType = sessionData.HouseType,
                    SubdivisionName = sessionData.SubdivisionName,
                    CityMunicipality = sessionData.CityMunicipality,
                    StreetName = sessionData.StreetName,
                    Province = sessionData.Province,
                    Phase = sessionData.Phase
                };
                _dbContext.Properties.Add(newProperty);
                _dbContext.SaveChanges();
                Console.WriteLine("✅ Property saved!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Database Insert Failed: {ex.Message}");
            }
        }
    }
}
