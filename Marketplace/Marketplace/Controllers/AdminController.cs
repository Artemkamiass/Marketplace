using Marketplace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Edit(long id)
        {
            var product = await _marketplaceContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["Categories"] = new SelectList(_marketplaceContext.Categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _marketplaceContext.Update(product);
                await _marketplaceContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["Categories"] = new SelectList(_marketplaceContext.Categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        public async Task<IActionResult> Delete(long id)
        {
            var product = await _marketplaceContext.Products.FindAsync(id);

            if (product != null)
            {
                _marketplaceContext.Products.Remove(product);
                await _marketplaceContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            await Console.Out.WriteLineAsync("Сработал индекс");
            return View(await _marketplaceContext.Products.Include(p => p.Category).ToListAsync());
        }

    }
}
