using E_Commers.Data;
using E_Commers.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Commers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext dbcontext = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = dbcontext.Products.ToList();

            return View(model: products);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Details(int productId)
        {
            var product = dbcontext.Products.Find(productId);
            if (product != null)
            {
                return View(model:product);

            }
            else 
            {
                return RedirectToAction(nameof(NotFoundPage));
            }

        }
        public IActionResult NotFoundPage()
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
