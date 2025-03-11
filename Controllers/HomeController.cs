using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Vista_Subdivision.Models;

namespace Vista_Subdivision.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
  
        public IActionResult Index() => SafeView("Index");
        public IActionResult About() => SafeView("About");
        public IActionResult Gallery() => SafeView("Gallery");
        public IActionResult Contact() => SafeView("Contact");
        public IActionResult Help() => SafeView("Help");
        public IActionResult LogIn() => SafeView("LogIn");
        public IActionResult Others() => SafeView("Others");

        private ViewResult SafeView(string viewName)
        {
            try { return View(viewName); }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {viewName} action");
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Error action");
                return View("Error");
            }
        }
    }
}
