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
        private readonly ILogger<EventCalendarModel> _logger;
        public EventCalendarModel(ApplicationDbContext context, ILogger<EventCalendarModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostUpdateEventAsync()
        {
            Console.Write("OnPostUpdateEvent Reached!"); // Debugging line
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                var updatedEvent = JsonSerializer.Deserialize<EventCalendar>(body);

                if (updatedEvent == null)
                {
                    return BadRequest("Invalid event data.");
                }

                // Fetch the existing event from the database
                var existingEvent = await _context.EventCalendars.FindAsync(updatedEvent.EventID);

                if (existingEvent == null)
                {
                    return NotFound("Event not found.");
                }

                // Update the event fields
                existingEvent.EventDate = updatedEvent.EventDate;
                existingEvent.Title = updatedEvent.Title;
                existingEvent.EventTime = updatedEvent.EventTime;
                existingEvent.EventType = updatedEvent.EventType;
                existingEvent.Description = updatedEvent.Description;

                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating event: " + ex.Message);
                return StatusCode(500, "An error occurred while updating the event.");
            }
        }

        public async Task<IActionResult> OnGetGetEventsAsync()
        {
            _logger.LogInformation("OnGetGetEventsAsync method was called.");
            Console.WriteLine("OnGetGetEventsAsync method was called.");
            try
            {
                var eventsIn = await _context.EventCalendars.ToListAsync();
                Console.WriteLine($"Retrieved {eventsIn.Count} events.");
                return new JsonResult(eventsIn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching events");
                return StatusCode(500, "An error occurred while fetching events.");
            }
        }   

        [HttpPost]
        public async Task<IActionResult> OnPostAddEventAsync([FromBody] EventCalendar newEvent)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                string userId = HttpContext.Session.GetString("Id");
                var OrganizerID = Convert.ToInt32(userId);

                if (newEvent == null || string.IsNullOrEmpty(newEvent.Title) || string.IsNullOrEmpty(newEvent.Description) || string.IsNullOrEmpty(newEvent.EventDate) || string.IsNullOrEmpty(newEvent.EventTime))
                {
                    return BadRequest("Invalid event data.");
                }

                var eventToSave = new EventCalendar
                {
                    Title = newEvent.Title,
                    Description = newEvent.Description,
                    EventDate = newEvent.EventDate,
                    EventTime = newEvent.EventTime,
                    EventType = newEvent.EventType,
                    OrganizerID = OrganizerID
                };

                _context.EventCalendars.Add(eventToSave);
                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while saving the event: " + ex.Message);
            }
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostDeleteEventAsync()
        {
            try
            {
                // Read the body of the POST request
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                // Parse the JSON body to get the eventId
                var requestData = JsonSerializer.Deserialize<DeleteEventRequest>(body);

                if (requestData == null || requestData.EventId == 0)
                {
                    Console.WriteLine("Invalid request data. EventId: " + requestData.EventId);
                    return BadRequest("Invalid event data.");
                }

                // Delete the event from the database
                var eventToDelete = await _context.EventCalendars.FindAsync(requestData.EventId);
                if (eventToDelete == null)
                {
                    return NotFound("Event not found.");
                }

                _context.EventCalendars.Remove(eventToDelete);
                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true });
            }
            catch (Exception e)
            {
                // You can log ex if needed
                return StatusCode(500, "An error occurred while deleting the event.");
            }
        }

        // Helper class to deserialize incoming JSON
        public class DeleteEventRequest
        {
            public int EventId { get; set; }
        }
    }
}