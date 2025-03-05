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
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Index action");
                return View("Error");
            }
        }

        public IActionResult About()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in About action");
                return View("Error");
            }
        }

        public IActionResult Gallery()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Gallery action");
                return View("Error");
            }
        }

        public IActionResult Contact()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Contact action");
                return View("Error");
            }
        }

        public IActionResult Help()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Help action");
                return View("Error");
            }
        }

        public IActionResult LogIn()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LogIn action");
                return View("Error");
            }
        }

        public IActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Register action");
                return View("Error");
            }
        }

        public IActionResult Register2()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Register2 action");
                return View("Error");
            }
        }

        public IActionResult Register3()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Register3 action");
                return View("Error");
            }
        }

        public IActionResult Others()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Others action");
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
