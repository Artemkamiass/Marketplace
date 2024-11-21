using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Marketplace.Controllers
{
    public class CategoryController : Controller
    {
        private readonly MarketplaceContext _marketplaceContext;

        public CategoryController(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _marketplaceContext.Category.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Category categories)
        {
            if (ModelState.IsValid)
            {
                _marketplaceContext.Add(categories);
                await _marketplaceContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categories);
        }


        public async Task<IActionResult> Edit(long id)
        {
            var categories = await _marketplaceContext.Category.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category categories)
        {
            if (ModelState.IsValid)
            {
                _marketplaceContext.Update(categories);
                await _marketplaceContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        public async Task<IActionResult> Delete(long id)
        {
            var category = await _marketplaceContext.Category.FindAsync(id);
            if (category != null)
            {
                _marketplaceContext.Category.Remove(category);
                await _marketplaceContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
