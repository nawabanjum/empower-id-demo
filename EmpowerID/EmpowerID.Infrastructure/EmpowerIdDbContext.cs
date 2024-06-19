using EmpowerID.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpowerID.Infrastructure
{
    public class EmpowerIdDbContext : DbContext
    {
        public EmpowerIdDbContext(DbContextOptions options)
    : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
              .HasIndex(x => new { x.Id, x.Name })
              .IsUnique();
            modelBuilder.Entity<Product>()
                .HasIndex(x => new { x.Category_Id, x.Product_id })
                .IsUnique();
            modelBuilder.Entity<OrderItem>().HasKey(x => new { x.OrderId, x.ProductId });


        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
