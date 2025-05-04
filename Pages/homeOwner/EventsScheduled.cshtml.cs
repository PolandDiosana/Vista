using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Pages.homeOwner
{
    [IgnoreAntiforgeryToken]
    public class EventsScheduledModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EventsScheduledModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetGetEventsAsync()
        {
            Console.WriteLine("OnGetGetEventsAsync method was called.");
            try
            {
                var eventsIn = await _context.EventCalendars.ToListAsync();
                Console.WriteLine($"Retrieved {eventsIn.Count} events.");
                return new JsonResult(eventsIn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching events.");
            }
        }
    }
}
