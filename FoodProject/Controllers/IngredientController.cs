using FoodProject.Data;
using FoodProject.Models;
using Microsoft.AspNetCore.Mvc;

public class IngredientController : Controller
{
    private readonly MenuContext _context;

    public IngredientController(MenuContext context)
    {
        _context = context;
    }

    public IActionResult AddIngredient()
    {
        var ingredients = _context.Ingredients.ToList();
        return View(ingredients);
    }

    [HttpPost]
    public IActionResult AddIngredient(Ingredient ingredient)
    {
        if (ModelState.IsValid)
        {
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();
            return RedirectToAction("AddIngredient");
        }

        var ingredients = _context.Ingredients.ToList();
        return View(ingredients);
    }

    [HttpDelete]
    public IActionResult DeleteIngredient(int id)
    {
        var ingredient = _context.Ingredients.Find(id);
        if (ingredient == null)
        {
            return Json(new { success = false });
        }

        _context.Ingredients.Remove(ingredient);
        _context.SaveChanges();

        return Json(new { success = true });
    }
}
