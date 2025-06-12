using FoodProject.Data;
using FoodProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace FoodProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MenuContext _context;

        public HomeController(ILogger<HomeController> logger, MenuContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var randomDishes = _context.Dishes
                .OrderBy(d => Guid.NewGuid())
                .Take(3)
                .ToList();

            var totalUsers = _context.Accounts.Count(a => a.Role == "User");

            var viewModel = new HomePageViewModel
            {
                Dishes = randomDishes,
                TotalUsers = totalUsers
            };

            return View(viewModel);
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
