using Bshare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bshare.Db;
using Microsoft.EntityFrameworkCore;

namespace Bshare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private BshareDbContext db = new BshareDbContext(new DbContextOptions<BshareDbContext>());

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(db.FileUploads.ToList());
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