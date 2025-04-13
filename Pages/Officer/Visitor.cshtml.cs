using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Pages.Officer
{
    [IgnoreAntiforgeryToken]
    public class VisitorModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public VisitorModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnPostVisitPassAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            Security Security;

            try
            {
                Security = JsonSerializer.Deserialize<Security>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid JSON", error = ex.Message });
            }

            if (Security.HomeownerID == 0 || string.IsNullOrWhiteSpace(Security.VisitorName))
            {
                return BadRequest(new { message = "Missing fields" });
            }

            if (Security.VehicleInfo is null)
            {
                Security.VehicleInfo = "Others";
            } 
            Security.EntryDate = DateOnly.FromDateTime(DateTime.Now);
            Security.EntryTime = TimeOnly.FromDateTime(DateTime.Now);

            _dbContext.Securities.Add(Security);
            await _dbContext.SaveChangesAsync();

            return new JsonResult(new
            {
                message = "Visitor pass recorded successfully.",
                data = new
                {
                    Security.SecurityID,
                    Security.HomeownerID,
                    Security.VisitorName,
                    Security.VehicleInfo,
                    EntryDate = Security.EntryDate.ToString("yyyy-MM-dd"),
                    EntryTime = Security.EntryTime.ToString("hh:mm tt")
                }
            });
        }

        public async Task<IActionResult> OnPostVisitorOutAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            Security request;

            try
            {
                request = JsonSerializer.Deserialize<Security>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid JSON", error = ex.Message });
            }

            if (request.HomeownerID == 0 || request.SecurityID == 0)
            {
                return BadRequest(new { message = "HomeownerID and SecurityID are required." });
            }

            var existingRecord = await _dbContext.Securities
                .FirstOrDefaultAsync(s => s.SecurityID == request.SecurityID && s.HomeownerID == request.HomeownerID);

            if (existingRecord == null)
            {
                return NotFound(new { message = "Visitor record not found." });
            }

            existingRecord.ExitDate = DateOnly.FromDateTime(DateTime.Now);
            existingRecord.ExitTime = TimeOnly.FromDateTime(DateTime.Now);

            await _dbContext.SaveChangesAsync();

            return new JsonResult(new
            {
                message = "Visitor exit recorded successfully.",
                data = new
                {
                    existingRecord.SecurityID,
                    existingRecord.HomeownerID,
                    existingRecord.VisitorName,
                    existingRecord.VehicleInfo,
                    EntryDate = existingRecord.EntryDate.ToString("yyyy-MM-dd"),
                    EntryTime = existingRecord.EntryTime.ToString("hh:mm tt"),
                    ExitDate = existingRecord.ExitDate?.ToString("yyyy-MM-dd"),
                    ExitTime = existingRecord.ExitTime?.ToString("hh:mm tt")
                }
            });
        }
    }
}
