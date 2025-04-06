using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Pages.homeOwner
{
    public class FacilityReservationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FacilityReservationModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public string Facility { get; set; }

        [BindProperty]
        public DateTime Date { get; set; }

        [BindProperty]
        public TimeOnly Time { get; set; }
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostReserveAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var data = JsonSerializer.Deserialize<FacilityReservation>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (data == null || string.IsNullOrEmpty(data.Facility) || data.Date == default || data.Time == default)
            {
                return BadRequest("Invalid reservation data.");
            }

            string userId = HttpContext.Session.GetString("Id");

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is not available.");
            }
            var user = Convert.ToInt32(userId);
            var reservation = new FacilityReservation
            {
                Id = user,
                Facility = data.Facility,
                Date = data.Date,
                Time = data.Time,
                Status = "Pending"
            };

            _context.FacilityReservations.Add(reservation);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
