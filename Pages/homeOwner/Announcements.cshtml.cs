using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Vista_Subdivision.Data;

namespace Vista_Subdivision.Pages.homeOwner
{
    public class AnnouncementsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public AnnouncementsModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Property to store user data
        public User LoggedInUser { get; set; }
        public async Task<IActionResult> OnGetAllAsync()
        {
            /*string userId = HttpContext.Session.GetString("Id");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Login");
            }

            // ✅ Fetch user from the database
            LoggedInUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (LoggedInUser == null)
            {
                return RedirectToPage("/Login");
            }*/

            var announcements = await _dbContext.Announcements.ToListAsync();

            if (announcements == null || announcements.Count == 0)
            {
                return NotFound(new { message = "No announcements found." });
            }

            return new JsonResult(new
            {
                message = "Announcements retrieved successfully.",
                data = announcements
            });
        }
    }
}
