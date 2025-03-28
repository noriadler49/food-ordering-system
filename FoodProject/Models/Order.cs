using System;
using System.Collections.Generic;

namespace FoodProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int AccountId { get; set; } // ✅ Link Order to AccountId
        public Account Account { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string Status { get; set; } = "Pending"; // ✅ Order status tracking
    }


    public class OrderItem
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}