using System.Diagnostics;
using APS_LostProperty.Areas.Identity.Data;
using APS_LostProperty.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APS_LostProperty.Controllers
{
    
    public class HomeController : Controller
    {
        // logger for debugging (already built-in)
        private readonly ILogger<HomeController> _logger;

        // database context (THIS WAS MISSING IN YOUR CODE)
        private readonly DBContext _context;

        // constructor injects BOTH logger and database
        public HomeController(ILogger<HomeController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        // HOME PAGE (DASHBOARD)
        public IActionResult Index()
        {
            // total number of lost items in system
            ViewBag.TotalItems = _context.LostItem.Count();

            // total claims submitted by users
            ViewBag.TotalClaims = _context.Claim.Count();

            // total categories available in system
            ViewBag.TotalCategories = _context.Category.Count();

            return View();
        }

        // PRIVACY PAGE (default scaffolded page)
        public IActionResult Privacy()
        {
            return View();
        }

        // ERROR HANDLING PAGE
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}