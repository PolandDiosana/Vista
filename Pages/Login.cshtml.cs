using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Vista_Subdivision.Data;

namespace Vista_Subdivision.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public LoginModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public void OnGet()
        {
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Console.WriteLine($"🔍 Debug: Username = '{Username}', Password = '{Password}'");

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    Console.WriteLine("❌ Validation Failed: Empty Username or Password.");
                    return new JsonResult(new { success = false, message = "Username and Password are required!" });
                }

                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == Username && u.PasswordHash == Password);

                if (user == null)
                {
                    Console.WriteLine("❌ Validation Failed: Invalid Username or Password.");
                    return new JsonResult(new { success = false, message = "Invalid Username or Password!" });
                }

                Console.WriteLine($"✅ User Found: {user.Username}");

                HttpContext.Session.SetString("Id", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                Console.WriteLine("✅ Login Successful. Redirecting...");

                return new JsonResult(new { success = true, message = "Login successfully", redirectUrl = GetRedirectUrl(user.Role) });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Server Error: {ex.Message}");
                return new JsonResult(new { success = false, message = "Server error occurred. Please check logs." });
            }
        }


        private string GetRedirectUrl(string role)
        {
            return role switch
            {
                "Admin" => "/Admin/adminDash",
                "Officer" => "/Officer/dashboard",
                _ => "/homeOwner/dashboard",
            };
        }

    }
}
