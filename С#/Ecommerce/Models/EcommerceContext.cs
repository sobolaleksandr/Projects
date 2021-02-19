using Microsoft.EntityFrameworkCore;
using Ecommerce.ViewModels;
using Ecommerce.Models;

namespace Ecommerce.Models
{
    public class EcommerceContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<LineBuffer> LineBuffers { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<ProductList> ProductLists { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<CustomerInvestment> CustomerInvestments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Product>()
            //    .HasAlternateKey(u => new { u.Name });//имя продукта - уникально
            //modelBuilder.Entity<Customer>()
            //    .HasAlternateKey(u => new { u.Email });//email клиента - уникален
            modelBuilder.Entity<LineBuffer>()
                .HasAlternateKey(u => new { u.ProductName });//В одном заказе уникальные продукты
            modelBuilder.Entity<Order>()
                .HasAlternateKey(u => new { u.Number });//номер заказа уникален

            modelBuilder.Entity<Product>().HasData(
            new Product[]
            {
                new Product { Name="mango", Price=230},
                new Product { Name="banana", Price=206},
                new Product { Name="apple", Price=100}
            });
        }
    }
}
