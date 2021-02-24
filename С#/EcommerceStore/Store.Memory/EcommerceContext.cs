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
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LineBuffer>()
                .HasAlternateKey(u => new { u.ProductName });//В одном заказе уникальные продукты
            modelBuilder.Entity<Order>()
                .HasAlternateKey(u => new { u.Number });//номер заказа уникален

            Product[] fruits = new Product[]
            {
                new Product { Name="mango", Price=230},
                new Product { Name="banana", Price=206},
                new Product { Name="apple", Price=100}
            };
            Customer[] employers = new Customer[]
            {
                new Customer { Name="Alex", Email = "alex99@gmail.com"},
                new Customer { Name="Bob", Email = "bob_bob@yandex.com"},
                new Customer { Name="Tom", Email = "TomT@yahoo.com"}
            };

            modelBuilder.Entity<Product>().HasData(fruits);
            modelBuilder.Entity<Customer>().HasData(employers);
        }
    }

}