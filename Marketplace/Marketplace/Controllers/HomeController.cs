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

        public async Task<IActionResult> Index()
        {
            var products = await _marketplaceContext.Products.Include(p => p.Category).ToListAsync();
            var cart = GetCart();
            foreach (var product in products)
            {
                var quantity = cart.FirstOrDefault(p => p.ProductId == product.Id)?.Quantity ?? 0;
                ViewData["ProductQuantity_" + product.Id] = quantity;
            }
            ViewData["QuantityItems"] = cart.Count;
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

            var results = _marketplaceContext.Products.Where(p => p.Name.Contains(query)).ToList();
            return RedirectToAction("Search", "Home", results);
        }
    }
}
