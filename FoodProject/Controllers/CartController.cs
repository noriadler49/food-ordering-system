using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FoodProject.Models;
using FoodProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodProject.Controllers
{
    [Authorize] // Ensure only logged-in users can access
    public class CartController : Controller
    {
        private readonly MenuContext _context;

        public CartController(MenuContext context)
        {
            _context = context;
        }

        // ✅ Display the cart (only for the logged-in user)
        public async Task<IActionResult> Index()
        {
            if (!int.TryParse(User.FindFirst("AccountId")?.Value, out int accountId))
            {
                return RedirectToAction("Login", "Account"); // ✅ Redirect if not logged in
            }

            var cartItems = await _context.CartItems
                .Include(ci => ci.Dish)
                .Where(ci => ci.AccountId == accountId) // ✅ Only fetch user's cart items
                .ToListAsync();

            ViewBag.TotalPrice = cartItems.Sum(ci => ci.Dish.Price * ci.Quantity);
            return View(cartItems);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int dishId, int quantity = 1)
        {
            if (!int.TryParse(User.FindFirst("AccountId")?.Value, out int accountId))
            {
                return RedirectToAction("Login", "Account"); // ✅ Redirect to login if not authenticated
            }

            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            if (dish == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.DishId == dishId && ci.AccountId == accountId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    DishId = dishId,
                    AccountId = accountId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var accountId = int.Parse(User.FindFirst("AccountId")?.Value);

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.AccountId == accountId); 

            if (cartItem == null)
            {
                return NotFound();
            }

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var accountId = int.Parse(User.FindFirst("AccountId")?.Value); 

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.AccountId == accountId);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
