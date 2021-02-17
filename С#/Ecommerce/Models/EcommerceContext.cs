using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models
{
    public class EcommerceContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
