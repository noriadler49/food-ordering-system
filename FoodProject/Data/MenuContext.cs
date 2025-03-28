using Microsoft.EntityFrameworkCore;
using FoodProject.Models;
namespace FoodProject.Data
{
    public class MenuContext : DbContext
    {
        public MenuContext(DbContextOptions<MenuContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishIngredient>().HasKey(di => new
            {
                di.DishId,
                di.IngredientId
            });
            modelBuilder.Entity<DishIngredient>()
                .HasOne(d => d.Dish)
                .WithMany(di => di.DishIngredients)
                .HasForeignKey(di => di.DishId);
            modelBuilder.Entity<DishIngredient>()
                .HasOne(i => i.Ingredient)
                .WithMany(di => di.DishIngredients)
                .HasForeignKey(i => i.IngredientId);

            modelBuilder.Entity<Dish>().HasData(
                new Dish
                {
                    Id = 1,
                    Name = "Pizza",
                    Price = 7.99, 
                    ImageUrl = "https://www.lottemart.vn/media/catalog/product/cache/0x0/0/4/0400285440001-6.jpg.webp"
                },
                new Dish
                {
                    Id = 2,
                    Name = "Burger",
                    Price = 3.99, 
                    ImageUrl = "https://media-cdn.tripadvisor.com/media/photo-s/17/32/f6/be/beef-burgur.jpg"
                },
                new Dish //New dish
                {
                    Id = 3,
                    Name = "Spagheti",
                    Price = 5.99,
                    ImageUrl = "https://img.taste.com.au/sE-X5HqY/taste/2024/03/5-ingredient-turbo-charged-spaghetti-recipe-196959-1.jpg"
                }
            );

            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient
                {
                    Id = 1,
                    Name = "Cheese"
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Tomato"
                },
                new Ingredient
                {
                    Id = 3,
                    Name = "Bread"
                },
                new Ingredient
                {
                    Id = 4,
                    Name = "Beef"
                },
                new Ingredient
                {
                    Id = 5,
                    Name = "Tomato Sauce"
                },
                new Ingredient
                {
                    Id = 6,
                    Name = "Spagheti"
                }
            );

            modelBuilder.Entity<DishIngredient>().HasData(
                new DishIngredient
                {
                    DishId = 1,
                    IngredientId = 1
                },
                new DishIngredient
                {
                    DishId = 1,
                    IngredientId = 2
                },
                new DishIngredient
                {
                    DishId = 1,
                    IngredientId = 3
                },
                new DishIngredient
                {
                    DishId = 1,
                    IngredientId = 5
                },
                new DishIngredient
                {
                    DishId = 2,
                    IngredientId = 1
                },
                new DishIngredient
                {
                    DishId = 2,
                    IngredientId = 2
                },
                new DishIngredient
                {
                    DishId = 2,
                    IngredientId = 3
                },
                new DishIngredient
                {
                    DishId = 2,
                    IngredientId = 4
                },
                new DishIngredient
                {
                    DishId = 3,
                    IngredientId = 1
                },
                new DishIngredient
                {
                    DishId = 3,
                    IngredientId = 5
                },
                new DishIngredient
                {
                    DishId = 3,
                    IngredientId = 6
                }
            );

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
