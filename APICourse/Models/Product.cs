namespace APICourse.Models
{
    public class Product
    {
        public int ProductId { get; set;}
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
