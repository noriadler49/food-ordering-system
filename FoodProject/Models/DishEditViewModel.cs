namespace FoodProject.Models
{
    public class DishEditViewModel
    {
        public Dish Dish { get; set; }
        public List<Ingredient> AllIngredients { get; set; } = new List<Ingredient>(); // Đặt giá trị mặc định
        public List<int> SelectedIngredientIds { get; set; } = new List<int>(); // Đảm bảo không bị null
    }

}
