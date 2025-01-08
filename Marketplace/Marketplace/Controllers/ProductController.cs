using Marketplace.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
    public class ProductController : Controller
    {
        private readonly MarketplaceContext _marketplaceContext;

        public ProductController(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _marketplaceContext.Products.Include(p => p.Category).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_marketplaceContext.Categories, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            await Console.Out.WriteLineAsync("Метод креэйт вызван");
            await Console.Out.WriteLineAsync($"CategoryId: {product.CategoryId}");
            foreach (var entry in ModelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    await Console.Out.WriteLineAsync($"{entry.Key}: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
            if (ModelState.IsValid)
            {
                _marketplaceContext.Add(product);
                await _marketplaceContext.SaveChangesAsync();
                await Console.Out.WriteLineAsync("Сохранение в бд произошло");
                return RedirectToAction("Index","Home");
            }
            ViewData["Categories"] = new SelectList(_marketplaceContext.Categories, "Id", "Name", product.CategoryId);
            await Console.Out.WriteLineAsync("Ошибка валидации модели");
            return View(product);
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
    }
}
