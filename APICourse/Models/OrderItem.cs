namespace APICourse.Models
{
    //Պատվերի ապրանքների ներկայացնում է
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } //Որ պատվերին է պատկանում այս ապրանքը
        public int ProductId { get; set; } //Ապրանքի ID
        public int Quantity { get; set; } //Ապրանքի քանակը
        public decimal UnitPrice { get; set; } //Ապրանքի միավորի գինը   
        public Order Order { get; set; } = null!; //Պատվերը, որի մեջ է այս ապրանքը  
        public Product Product { get; set; } = null!; //Ապրանքը, որը պատկանում է այս պատվերին
    }
}
