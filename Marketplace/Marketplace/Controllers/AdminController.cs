using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
    public class AdminController : Controller
    {
        private readonly MarketplaceContext _marketplaceContext;
        public AdminController(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _marketplaceContext.Products.Include(p => p.Category).ToListAsync());
        }

    }
}
