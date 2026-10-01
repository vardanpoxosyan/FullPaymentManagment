namespace APICourse.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public Cart Cart { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = [];

    }
}
