using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PortalKsiazkowy.Models;

namespace PortalKsiazkowy.Controllers
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
            if (User.Identity.IsAuthenticated)
            {
                // Je�li u�ytkownik jest zalogowany, przekieruj na stron� g��wn� dla zalogowanych
                return RedirectToAction("Welcome");
            }
            else
            {
                // Je�li u�ytkownik nie jest zalogowany, wy�wietl widok domy�lny
                return View();
            }
        }

        // Widok dla zalogowanych u�ytkownik�w
        public IActionResult Welcome()
        {
            return View();
        }

        

        public IActionResult Privacy()
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
