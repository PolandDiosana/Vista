using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Pages.Officer
{
    [IgnoreAntiforgeryToken]
    public class AnnouncementModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnPostCreateAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                return BadRequest(new { message = "Empty request body" });
            }

            Announcement announcement;

            try
            {
                announcement = JsonSerializer.Deserialize<Announcement>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid JSON", error = ex.Message });
            }

            // Validate required fields manually
            if (string.IsNullOrWhiteSpace(announcement.Title) || string.IsNullOrWhiteSpace(announcement.Content) || announcement.AuthorID == 0)
            {
                return BadRequest(new { message = "Missing fields" });
            }

            // Set DatePosted automatically
            announcement.DatePosted = DateOnly.FromDateTime(DateTime.Today);

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                message = "Announcement created",
                data = announcement
            });
        }
        public async Task<IActionResult> OnGetAllAsync()
        {
            var announcements = await _context.Announcements.ToListAsync();

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
