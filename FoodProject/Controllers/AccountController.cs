using Microsoft.AspNetCore.Mvc;
using FoodProject.Models;
using FoodProject.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FoodProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly MenuContext _context;

        public AccountController(MenuContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Account account)
        {
            try
            {
                // Don't even check ModelState - just try to save
                account.Role = "User";
                account.Password = HashPassword(account.Password);

                _context.Accounts.Add(account);

                try
                {
                    var result = _context.SaveChanges();
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    return Content($"Database error: {ex.Message}. Inner: {ex.InnerException?.Message}");
                }
            }
            catch (Exception ex)
            {
                return Content($"General error: {ex.Message}");
            }
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Account account, string? returnUrl = null)
        {
            try
            {
                if (string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password))
                {
                    return Content("Error: Username and password are required.");
                }

                string hashedPassword = HashPassword(account.Password);
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == account.Username && u.Password == hashedPassword);

                if (user != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // ✅ Store AccountId here
                new Claim("AccountId", user.Id.ToString()), // ✅ This ensures Cart & Orders work
                new Claim(ClaimTypes.Role, user.Role)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Menu");
                }
                else
                {
                    return Content("Error: Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                return Content($"Login failed: {ex.Message}");
            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Menu");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}