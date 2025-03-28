using Microsoft.AspNetCore.Mvc;
using FoodProject.Data;
using FoodProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace FoodProject.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuContext _context;

        public MenuController(MenuContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult AddDish()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddDish(Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Dishes.Add(dish);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dish);
        }
        public async Task<IActionResult> Index(string searchString, string category = "All")
        {
            var dishes = _context.Dishes.AsQueryable();

            // Apply category filter if it's not "All"
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                dishes = dishes.Where(d => d.Category == category);
            }

            // Apply search filter if searchString is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                dishes = dishes.Where(d => d.Name.Contains(searchString));
            }

            // Store the selected category in ViewBag for UI use
            ViewBag.SelectedCategory = category;
            ViewBag.SearchString = searchString;

            return View(await dishes.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            var dish = await _context.Dishes
                .Include(di => di.DishIngredients)
                .ThenInclude(i => i.Ingredient)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }
        public IActionResult Edit(int id)
        {
            var dish = _context.Dishes
                               .Include(d => d.DishIngredients)
                               .ThenInclude(di => di.Ingredient)
                               .FirstOrDefault(d => d.Id == id);

            if (dish == null) return NotFound();

            var ingredients = _context.Ingredients.ToList();

            var viewModel = new DishEditViewModel
            {
                Dish = dish,
                AllIngredients = ingredients ?? new List<Ingredient>(), // Đảm bảo không bị null
                SelectedIngredientIds = dish.DishIngredients?.Select(di => di.IngredientId).ToList() ?? new List<int>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(DishEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dish = _context.Dishes.Include(d => d.DishIngredients)
                                      .FirstOrDefault(d => d.Id == model.Dish.Id);
            if (dish == null) return NotFound();

            // Update dish properties
            dish.Name = model.Dish.Name;
            dish.Description = model.Dish.Description;
            dish.ImageUrl = model.Dish.ImageUrl;
            dish.Price = model.Dish.Price;

            // Update Ingredients
            dish.DishIngredients.Clear();
            if (model.SelectedIngredientIds != null)
            {
                foreach (var ingredientId in model.SelectedIngredientIds)
                {
                    dish.DishIngredients.Add(new DishIngredient
                    {
                        DishId = dish.Id,
                        IngredientId = ingredientId
                    });
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Details", "Menu", new { id = dish.Id });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dish = _context.Dishes.Find(id);
            if (dish == null) return NotFound();

            _context.Dishes.Remove(dish);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
