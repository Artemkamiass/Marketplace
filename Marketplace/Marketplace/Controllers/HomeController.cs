using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;


namespace Marketplace.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly MarketplaceContext _marketplaceContext;
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        public HomeController(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _marketplaceContext.Products.Include(p => p.Category).ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
