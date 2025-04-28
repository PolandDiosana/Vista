using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Pages.homeOwner
{
    [IgnoreAntiforgeryToken]
    public class EditProfileModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public EditProfileModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public User Profile { get; set; }

        public async Task<IActionResult> OnPostEditProfileAsync([FromBody] User incoming)
        {
            if (incoming == null || incoming.Id == 0)
                return BadRequest(new { message = "Invalid user data." });

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == incoming.Id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            if (!string.IsNullOrWhiteSpace(incoming.FirstName))
                user.FirstName = incoming.FirstName;

            if (!string.IsNullOrWhiteSpace(incoming.Email))
                user.Email = incoming.Email;

            if (!string.IsNullOrWhiteSpace(incoming.LastName))
                user.LastName = incoming.LastName;

            if (!string.IsNullOrWhiteSpace(incoming.PhoneNumber))
                user.PhoneNumber = incoming.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(incoming.Username))
                user.Username = incoming.Username;

            if (!string.IsNullOrWhiteSpace(incoming.PasswordHash))
                user.PasswordHash = incoming.PasswordHash;

            await _dbContext.SaveChangesAsync();

            return new JsonResult(new
            {
                message = "Profile updated successfully.",
                data = user
            });
        }
    }
}
