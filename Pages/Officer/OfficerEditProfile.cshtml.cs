using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Pages.Officer
{
	[IgnoreAntiforgeryToken]
	public class OfficerEditProfileModel : PageModel
	{
		private readonly ApplicationDbContext _dbContext;

		public OfficerEditProfileModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public class EditProfilePayload
		{
			public User User { get; set; }
			public EmergencyContact EmergencyContact { get; set; }
			public Property Property { get; set; }
		}

		[BindProperty]
		public User LoggedInUser { get; set; }

		[BindProperty]
		public Property Properties { get; set; }

		[BindProperty]
		public EmergencyContact EmergencyContact { get; set; }

		public async Task<IActionResult> OnPostEditProfileAsync([FromBody] EditProfilePayload payload)
		{
			Console.WriteLine($"Received payload: {System.Text.Json.JsonSerializer.Serialize(payload)}");

			if (payload == null || payload.User == null || payload.User.Id == 0)
			{
				Console.WriteLine("Invalid payload: payload is null or user ID is 0");
				return BadRequest(new { message = "Invalid user data." });
			}

			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == payload.User.Id);
			if (user == null)
			{
				Console.WriteLine($"User not found with ID: {payload.User.Id}");
				return NotFound(new { message = "User not found." });
			}

			Console.WriteLine($"Found existing user: {System.Text.Json.JsonSerializer.Serialize(user)}");

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

			Console.WriteLine($"Updated user data: {System.Text.Json.JsonSerializer.Serialize(user)}");

			// Explicitly mark as modified
			_dbContext.Entry(user).State = EntityState.Modified;

			// Handle EmergencyContact
			if (payload.EmergencyContact != null)
			{
				var ec = await _dbContext.EmergencyContacts.FirstOrDefaultAsync(e => e.UserId == incoming.Id);
				if (ec != null)
				{
					Console.WriteLine($"Found existing emergency contact: {System.Text.Json.JsonSerializer.Serialize(ec)}");
					ec.ContactName = payload.EmergencyContact.ContactName;
					ec.PhoneNumber = payload.EmergencyContact.PhoneNumber;
					ec.Relationship = payload.EmergencyContact.Relationship;
					ec.Email = payload.EmergencyContact.Email;

					_dbContext.Entry(ec).State = EntityState.Modified;
					Console.WriteLine($"Updated emergency contact: {System.Text.Json.JsonSerializer.Serialize(ec)}");
				}
				else
				{
					Console.WriteLine("Creating new emergency contact");
					payload.EmergencyContact.UserId = incoming.Id;
					_dbContext.EmergencyContacts.Add(payload.EmergencyContact);
				}
			}

			// Handle Property
			if (payload.Property != null)
			{
				var prop = await _dbContext.Properties.FirstOrDefaultAsync(p => p.UserId == incoming.Id);
				if (prop != null)
				{
					Console.WriteLine($"Found existing property: {System.Text.Json.JsonSerializer.Serialize(prop)}");
					prop.SubdivisionName = payload.Property.SubdivisionName;
					prop.CityMunicipality = payload.Property.CityMunicipality;

					_dbContext.Entry(prop).State = EntityState.Modified;
					Console.WriteLine($"Updated property: {System.Text.Json.JsonSerializer.Serialize(prop)}");
				}
				else
				{
					Console.WriteLine("Creating new property");
					payload.Property.UserId = incoming.Id;
					_dbContext.Properties.Add(payload.Property);
				}
			}

			// Check if there are any pending changes
			var pendingChanges = _dbContext.ChangeTracker.HasChanges();
			Console.WriteLine($"Has pending changes: {pendingChanges}");

			if (!pendingChanges)
			{
				Console.WriteLine("No changes detected in the data");
				return BadRequest(new { message = "No changes detected in the data." });
			}

			try
			{
				await _dbContext.SaveChangesAsync();
				Console.WriteLine("Successfully saved changes to database");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error saving changes: {ex.Message}");
				return StatusCode(500, new { message = "Error saving changes to database." });
			}

			return new JsonResult(new
			{
				message = "Profile and related data updated successfully.",
				data = user
			});
		}

		public async Task<IActionResult> OnGetAsync()
		{
			string userIdStr = HttpContext.Session.GetString("Id");

			if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
			{
				return RedirectToPage("/Login");
			}

			Properties = await _dbContext.Properties
				.FirstOrDefaultAsync(p => p.UserId == userId);

			LoggedInUser = await _dbContext.Users
				.FirstOrDefaultAsync(u => u.Id == userId);

			EmergencyContact = await _dbContext.EmergencyContacts
				.FirstOrDefaultAsync(ec => ec.UserId == userId) ?? new EmergencyContact();

			return Page();
		}
	}
}