using Marketplace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly MarketplaceContext _marketplaceContext;       
        public AdminController(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }
        public async Task<IActionResult> Index()
        {
            await Console.Out.WriteLineAsync("Сработал индекс");
            //return View(await _marketplaceContext.Products.Include(p => p.Category).ToListAsync());
            return View();
        }

    }
}
