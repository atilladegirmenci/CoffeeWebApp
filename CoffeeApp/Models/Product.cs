using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;
        public string? ImageUrl { get; set; }
    }
}
