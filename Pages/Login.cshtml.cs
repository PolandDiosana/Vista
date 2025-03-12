using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;
using System.Threading.Tasks;

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
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Invalid form submission!";
                return Page();
            }

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Username and Password are required.";
                return Page();
            }

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == Username && u.PasswordHash == Password);

            if (user == null)
            {
                ErrorMessage = "Invalid Username or Password!";
                return Page();
            }

            Console.WriteLine($"✅ User Found: {user.Username}"); // FOR DEBUGGING PURPOSES

            // ✅ Store session after login
            HttpContext.Session.SetString("Id", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            // 🔄 Regular form submission (redirect)
            return RedirectToPage(GetRedirectUrl(user.Role));
        }

        // 🔥 Redirect Function
        private string GetRedirectUrl(string role)
        {
            return role switch
            {
                "Admin" => "/Admin/adminDash",
                "Staff" => "/Officer/dashboard",
                _ => "/homeOwner/dashboard",
            };
        }

    }
}
