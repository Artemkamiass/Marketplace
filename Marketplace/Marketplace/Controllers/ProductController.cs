using Marketplace.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Marketplace.Controllers
{
    public class ProductController : Controller
    {
        private readonly MarketplaceContext _marketplaceContext;
        private readonly IMapper _mapper;// Внедряем AutoMapper

        public ProductController(MarketplaceContext marketplaceContext, IMapper mapper)
        {
            _marketplaceContext = marketplaceContext;
            _mapper = mapper; //инициализация AutoMapper
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
        public async Task<IActionResult> Create(ProductCreateDTO productDto)
        {
            if(productDto.Photo == null || productDto.Photo.Length == 0)
            {
                ModelState.AddModelError("Photo", "Фотография обязательна для загрузки");
            }

            await Console.Out.WriteLineAsync("Метод креэйт вызван");

            await Console.Out.WriteLineAsync($"CategoryId: {productDto.CategoryId}");

            foreach (var entry in ModelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    await Console.Out.WriteLineAsync($"{entry.Key}: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(productDto); // Маппинг из DTO в сущность 

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image");

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + productDto.Photo.FileName;

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await productDto.Photo.CopyToAsync(fileStream);
                }

                product.PhotoPath = $"/image/{uniqueFileName}";

                _marketplaceContext.Add(product);

                await _marketplaceContext.SaveChangesAsync();

                await Console.Out.WriteLineAsync("Сохранение в бд произошло");

                return RedirectToAction("Index","Home");
            }

            ViewData["Categories"] = new SelectList(_marketplaceContext.Categories, "Id", "Name", productDto.CategoryId);

            await Console.Out.WriteLineAsync("Ошибка валидации модели");

            return View(productDto);
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
        
        public async Task<IActionResult> Details(long id)
        {
            var product = _marketplaceContext.Products.Find(id);

            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }
    }
}
