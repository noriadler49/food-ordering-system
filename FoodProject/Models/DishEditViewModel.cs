namespace FoodProject.Models
{
    public class DishEditViewModel
    {
        public Dish Dish { get; set; }
        public List<Ingredient> AllIngredients { get; set; } = new List<Ingredient>();
        public List<int> SelectedIngredientIds { get; set; } = new List<int>();
    }

}
