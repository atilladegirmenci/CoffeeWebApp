namespace CoffeeApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderCreated { get; set; } 
        public DateTime? OrderFullfilled { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
