using APICourse.Models;
using Microsoft.EntityFrameworkCore;

namespace APICourse.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options)
    {
        public DbSet<User> Users { get; set;}
        public DbSet<Cart> Carts { get; set;}
        public DbSet<CartItem> CartItems { get; set;}
        public DbSet<Product>   Products { get; set;}
        public DbSet<Order>   Orders { get; set;}
        public DbSet<OrderItem> OrderItems { get; set;}
        public DbSet<Payment> Payments { get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().Property(s => s.Price).HasPrecision(18,2);
          
            modelBuilder.Entity<Product>().HasMany(s => s.CartItems).
                WithOne(s => s.Product).HasForeignKey(s=>s.ProductId);
          
            modelBuilder.Entity<Cart>().HasOne(s => s.User)
                .WithOne(s => s.Cart).HasForeignKey<Cart>(s=>s.UserId);

            modelBuilder.Entity<Cart>().HasMany(s => s.CartItems).WithOne(s => s.Cart).HasForeignKey(s => s.CartId);
           
            modelBuilder.Entity<Order>()
              .HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
             .HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderItem>()
             .HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Payment>().HasOne(s=>s.Order).WithOne(s=>s.Payment).HasForeignKey<Payment>(s => s.OrderId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
