namespace FoodProject.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public List<DishIngredient>? DishIngredients { get; set; }
        public string Category { get; set; } = "Food";

    }
}
