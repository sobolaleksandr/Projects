using Microsoft.EntityFrameworkCore;
using Store.Views;

namespace Store.Memory
{
    public class EcommerceContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<LineBuffer> LineBuffers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<ProductList> ProductLists { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<CustomerInvestment> CustomerInvestments { get; set; }

        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildProducts(modelBuilder);
            BuildCustomers(modelBuilder);
            BuildOrders(modelBuilder);
            BuildLineBuffers(modelBuilder);
        }

        private static void BuildProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(action =>
            {
                action.HasData(
                new Product { Name = "mango", Price = 230 },
                new Product { Name = "banana", Price = 206 },
                new Product { Name = "apple", Price = 100 }
                );
            });
        }

        private static void BuildCustomers(ModelBuilder modelBuilder)
        {

        }

        private static void BuildLineBuffers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LineBuffer>(action =>
            {
                action.HasAlternateKey(u => new { u.ProductName });//В одном заказе уникальные продукты
            });
        }

        private static void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(action =>
            {
                action.HasAlternateKey(u => new { u.Number });//номер заказа уникален
            });
        }
    }

}