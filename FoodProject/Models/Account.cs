using System.ComponentModel.DataAnnotations;
namespace FoodProject.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required]
        public required string Role { get; set; }
    }
}