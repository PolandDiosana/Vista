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

        public class EditProfilePayload
        {
            public User User { get; set; }
            public EmergencyContact EmergencyContact { get; set; }
            public Property Property { get; set; } 
        }

        public async Task<IActionResult> OnPostEditProfileAsync([FromBody] EditProfilePayload payload)
        {
            if (payload == null || payload.User == null || payload.User.Id == 0)
                return BadRequest(new { message = "Invalid user data." });

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == payload.User.Id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            // Update User fields
            var incoming = payload.User;
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

            // Handle EmergencyContact (Update or Insert)
            if (payload.EmergencyContact != null)
            {
                var ec = await _dbContext.EmergencyContacts
                          .FirstOrDefaultAsync(e => e.UserId == incoming.Id);

                if (ec != null)
                {
                    ec.ContactName = payload.EmergencyContact.ContactName;
                    ec.PhoneNumber = payload.EmergencyContact.PhoneNumber;
                    ec.Relationship = payload.EmergencyContact.Relationship;
                    ec.Email = payload.EmergencyContact.Email;
                }
                else
                {
                    payload.EmergencyContact.UserId = incoming.Id;
                    _dbContext.EmergencyContacts.Add(payload.EmergencyContact);
                }
            }

            // Optionally handle Property (Update or Insert)
            if (payload.Property != null)
            {
                var prop = await _dbContext.Properties
                             .FirstOrDefaultAsync(p => p.UserId == incoming.Id);

                if (prop != null)
                {
                    prop.SubdivisionName = payload.Property.SubdivisionName;
                    prop.CityMunicipality = payload.Property.CityMunicipality;
                }
                else
                {
                    payload.Property.UserId = incoming.Id;
                    _dbContext.Properties.Add(payload.Property);
                }
            }

            await _dbContext.SaveChangesAsync();

            return new JsonResult(new
            {
                message = "Profile and related data updated successfully.",
                data = user
            });
        }


        public User LoggedInUser { get; set; }
        public Property Properties { get; set; }
        public EmergencyContact EmergencyContact { get; set; }

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

            EmergencyContact = await _dbContext.EmergencyContacts
                .FirstOrDefaultAsync(up => up.UserId.ToString() == userId) ?? new EmergencyContact();

            return Page();
        }
    }
}
