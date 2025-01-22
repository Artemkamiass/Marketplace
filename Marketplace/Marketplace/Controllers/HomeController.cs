using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Marketplace.Controllers
{
    public class HomeController : Controller
    {
        private const string _cartSessionKey = "Cart";    
        //private readonly ILogger<HomeController> _logger;
        private readonly MarketplaceContext _marketplaceContext;
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private List<CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetString(_cartSessionKey);
            return cart == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        public HomeController(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }

        public async Task<IActionResult> Index(long? categoryId, decimal? min, decimal? max)
        {   
            var productsQuery = _marketplaceContext.Products.Include(prQuery =>  prQuery.Category).AsQueryable();
            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(q => q.CategoryId == categoryId.Value);
            }

            if (min.HasValue)
            {
                productsQuery =  productsQuery.Where(q => q.Price >= min.Value);
            }

            if (max.HasValue)
            {
                productsQuery = productsQuery.Where(q => q.Price <= max.Value);
            }

            var products = await productsQuery.ToListAsync();
            var cart = GetCart();

            foreach (var product in products)
            {
                var quantity = cart.FirstOrDefault(p => p.ProductId == product.Id)?.Quantity ?? 0;
                ViewData["ProductQuantity_" + product.Id] = quantity;
            }

            ViewData["QuantityItems"] = cart.Count;
            ViewData["Categories"] = await _marketplaceContext.Categories.ToListAsync();
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            var results = _marketplaceContext.Products.ToList().Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase )).ToList();
            return View("Search", results);
        }
    }
}
