namespace APICourse.Models
{
    public enum OrderStatus
    {
        Pending,//պատվերը ստեղծվել է, բայց դեռ չի վճարվել
        Paid,//վճարումը կատարվել է
        Shipped,//Պատվերը ուղարկվել է, 
        Completed,//պատվերը հաջողությամբ ավարտվել է
        Cancelled//պատվերը չեղարկվել է
    }
    public class Order
    {
        public int Id { get; set; }
        //Որ User ին է պատկանում այս պատվերը
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }//երբ է ստեղծվել պատվերը
        public decimal TotalAmount { get; set; } //Պատվերի ընդհանուր գումարը
        public OrderStatus Status { get; set; }//Պատվերի կարգավիճակը
        public User User { get; set; } = null!;//Պատվերը ստեղծող օգտատերը
        public ICollection<OrderItem> OrderItems { get; set; } = [];//Պատվերի ապրանքների հավաքածու
        public Payment Payment { get; set; } = null!;//Պատվերի վճարման տեղեկատվությունը
    }
}
