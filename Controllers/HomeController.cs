using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Gallery()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Help()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Register2()
        {
            return View();
        }
        public IActionResult Register3()
        {
            return View();
        }
        public IActionResult Others()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
