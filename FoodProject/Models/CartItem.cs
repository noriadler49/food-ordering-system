using System.ComponentModel.DataAnnotations.Schema;

namespace FoodProject.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}