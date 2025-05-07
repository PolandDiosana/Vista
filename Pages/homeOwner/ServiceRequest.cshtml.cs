using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vista_Subdivision.Data;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Pages.homeOwner
{
    public class ServiceRequestModel : PageModel
    {
        private readonly ApplicationDbContext _context; 
        public ServiceRequestModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ServiceRequest Request { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                string userId = HttpContext.Session.GetString("Id");
                var UserID = Convert.ToInt32(userId);

                if (string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("User ID is null or empty in session");
                    return BadRequest("User session not found. Please log in again.");
                }

                Request.UserID = UserID;
                Request.Status = "Pending";
                Request.DateSubmitted = DateTime.Now.ToString("MMMM dd, yyyy");

                Console.WriteLine();
                Console.WriteLine("Service Request Data: ");
                Console.WriteLine($"Request ID: {Request.RequestID}");
                Console.WriteLine($"User ID: {Request.UserID}");
                Console.WriteLine($"Req Title: {Request.ReqTitle}");
                Console.WriteLine($"Location: {Request.Location}");
                Console.WriteLine($"Street Name: {Request.StreetName}");
                Console.WriteLine($"Priority Level: {Request.PriorityLevel}");
                Console.WriteLine($"Description: {Request.Description}");
                Console.WriteLine($"Category: {Request.Category}");
                Console.WriteLine($"Date Submitted: {Request.DateSubmitted}");
                Console.WriteLine($"Status: {Request.Status}");
                Console.WriteLine();

                // Debug: Check if ModelState is valid and list errors
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid. Errors:");
                    return BadRequest(ModelState);
                }

                _context.ServiceRequest.Add(Request);
                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true, message = "Service request submitted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }
        }
    }
}
