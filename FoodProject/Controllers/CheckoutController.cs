using Microsoft.AspNetCore.Mvc;
using FoodProject.Data;
using FoodProject.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FoodProject.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly MenuContext _context;

        public CheckoutController(MenuContext context)
        {
            _context = context;
            
        }
        
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Dish)
                .ToListAsync();

            var totalPrice = cartItems.Sum(item => item.Quantity * item.Dish.Price);

            ViewBag.TotalPrice = totalPrice;

            return View();
        }


        [Authorize]
        public async Task<IActionResult> MyOrders(string status = "All")
        {
            var accountId = int.Parse(User.FindFirst("AccountId")?.Value);

            var ordersQuery = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Where(o => o.AccountId == accountId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                ordersQuery = ordersQuery.Where(o => o.Status == status);
            }

            ViewBag.SelectedStatus = status;
            return View(await ordersQuery.ToListAsync());
        }

        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Orders(string status = "All")
        {
            var ordersQuery = _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                ordersQuery = ordersQuery.Where(o => o.Status == status);
            }

            ViewBag.SelectedStatus = status;
            return View(await ordersQuery.ToListAsync());
        }


        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var accountId = int.Parse(User.FindFirst("AccountId")?.Value);

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Dish)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.AccountId == accountId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            order.Status = newStatus;
            await _context.SaveChangesAsync();

            return RedirectToAction("Orders");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ConfirmOrderReceived(int orderId)
        {
            var accountId = int.Parse(User.FindFirst("AccountId")?.Value);
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.AccountId == accountId);

            if (order == null || order.Status != "Delivering")
            {
                return BadRequest("Order not found or not eligible for confirmation.");
            }

            order.Status = "Received";
            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var accountId = int.Parse(User.FindFirst("AccountId")?.Value);
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.AccountId == accountId);

            if (order == null || order.Status != "Pending")
            {
                return BadRequest("Order not found or not eligible for confirmation.");
            }

            order.Status = "Canceled";
            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders");
        }
        [HttpPost]
        public async Task<IActionResult> ApplyVoucher(string code)
        {
            var accountId = int.Parse(User.FindFirst("AccountId")?.Value);
            var cartItems = await _context.CartItems
                .Include(c => c.Dish)
                .Where(c => c.AccountId == accountId)
                .ToListAsync();

            var total = cartItems.Sum(item => item.Quantity * item.Dish.Price);

            var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == code);
            if (voucher != null)
            {
                var discountAmount = total * voucher.Discount / 100;
                var discountedTotal = total - discountAmount;

                return Json(new { success = true, discountedTotal = discountedTotal.ToString("N2"), code = voucher.Code });
            }

            return Json(new { success = false, message = "Invalid voucher code." });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string name, string address, string payment, string usedVoucher)
        {
            var accountId = int.Parse(User.FindFirst("AccountId")?.Value);

            var cartItems = await _context.CartItems
                .Include(c => c.Dish)
                .Where(c => c.AccountId == accountId)
                .ToListAsync();

            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            var total = cartItems.Sum(item => item.Quantity * item.Dish.Price);
            string appliedVoucher = null;

            if (!string.IsNullOrEmpty(usedVoucher))
            {
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.Code == usedVoucher);
                if (voucher != null)
                {
                    total -= total * voucher.Discount / 100;
                    appliedVoucher = voucher.Code;
                }
            }

            var order = new Order
            {
                AccountId = accountId,
                TotalPrice = total,
                VoucherCode = appliedVoucher
            };

            foreach (var cartItem in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    DishId = cartItem.DishId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Dish.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }

    }
}