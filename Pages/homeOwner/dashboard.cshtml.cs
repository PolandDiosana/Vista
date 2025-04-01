using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;
using System.Threading.Tasks;

namespace Vista_Subdivision.Pages.homeOwner
{
    public class dashboardModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public dashboardModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int ContactCount { get; set; }
        public List<User> Contacts { get; set; }
        public User LoggedInUser { get; set; }
        public Property Properties { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string userId = HttpContext.Session.GetString("Id");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Login");
            }
            Properties = await _dbContext.Properties
                .FirstOrDefaultAsync(p => p.UserId.ToString() == userId);

            LoggedInUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (LoggedInUser == null)
            {
                return RedirectToPage("/Login");
            }

            ContactCount = await _dbContext.Users
                .Where(u => (u.Role == "Homeowner" || u.Role == "Officer") && u.Id.ToString() != userId)
                .CountAsync();

            Contacts = await _dbContext.Users
                .Where(u => (u.Role == "Homeowner" || u.Role == "Officer") && u.Id.ToString() != userId)
                .OrderBy(u => u.Role == "Homeowner")
                .ToListAsync();

            return Page();
        }
    }
}
