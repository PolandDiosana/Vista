using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Pages.Officer
{
    [IgnoreAntiforgeryToken]
    public class EventCalendarModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EventCalendarModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAddEventAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(body))
                return BadRequest("Empty body");

            EventCalendar eventCalendar;

            eventCalendar = JsonSerializer.Deserialize<EventCalendar>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (eventCalendar == null)
                return BadRequest("Invalid event data");

            _context.EventCalendars.Add(eventCalendar);
            await _context.SaveChangesAsync();

            return new JsonResult(eventCalendar);
        }
    }
}
