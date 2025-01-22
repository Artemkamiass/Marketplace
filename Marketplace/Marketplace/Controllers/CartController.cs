using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace Marketplace.Controllers
{
    public class CartController : Controller
    {
        private const string _cartSessionKey = "Cart";

        private List<CartItem> GetCart() 
        { 
            var cart = HttpContext.Session.GetString(_cartSessionKey);
            return cart == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }
        
        private void SaveCart(List<CartItem> cartItem)
        {
            HttpContext.Session.SetString(_cartSessionKey, JsonConvert.SerializeObject(cartItem));
        }

        private Product GetProductById(long productId)
        {
            return HttpContext.RequestServices.GetService<MarketplaceContext>()?.Products
                .FirstOrDefault(product => product.Id == productId);
        }

        public IActionResult AddToCart(long productId)
        {
            var product = GetProductById(productId);

            if(product == null)
            {
                return NotFound();
            }

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if(existingItem != null)
            {
                existingItem.Quantity++ ;
            }
            else
            {
                cart.Add(new CartItem()
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    PhotoPath = product.PhotoPath
                }) ;
            }

            SaveCart(cart);

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult RemoveFromCart(long productId)
        {
            var cart = GetCart();

            var itemToRemove = cart.FirstOrDefault(c => c.ProductId == productId);

            if(itemToRemove != null)
            {
                cart.Remove(itemToRemove);
            }

            SaveCart(cart);
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(long productId, int value)
        {
            var cart = GetCart();

            var itemToRemove = cart.FirstOrDefault(c => c.ProductId == productId);
            if(itemToRemove != null)
            {
                Console.WriteLine(itemToRemove.Quantity);
                itemToRemove.Quantity += value;
                Console.WriteLine(itemToRemove.Quantity);
                if (itemToRemove.Quantity <= 0)
                {
                    cart.Remove(itemToRemove);
                }
            }        

            SaveCart(cart);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }
    }
}
