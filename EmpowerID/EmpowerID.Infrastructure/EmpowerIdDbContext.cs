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

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
