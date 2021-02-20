using Microsoft.EntityFrameworkCore;
using Ecommerce.ViewModels;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class EcommerceContext:DbContext
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
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

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
            //Order[] orders = new Order[]
            //{
            //    new Order { Id = 2, Created=System.DateTime.Now, Number=1,
            //    CustomerEmail = employers[0].Email,
            //        Customer = employers[0], Items = new List<LineBuffer>()
            //    {new LineBuffer{ProductName = fruits[1].Name,Quantity = 100} }
            //    },
            //    new Order { Id = 3, Created=System.DateTime.Now, Number=2,
            //    CustomerEmail = employers[0].Email,
            //        Customer = employers[0], Items = new List<LineBuffer>()
            //    {new LineBuffer{ProductName = fruits[0].Name,Quantity = 10} }
            //    },
            //    new Order { Id = 4, Created=System.DateTime.Now, Number=3,
            //    CustomerEmail = employers[2].Email,
            //        Customer = employers[2], Items = new List<LineBuffer>()
            //    {new LineBuffer{ProductName = fruits[0].Name,Quantity = 10} }
            //    }
            //};

            modelBuilder.Entity<Product>().HasData(fruits);
            modelBuilder.Entity<Customer>().HasData(employers);
            //modelBuilder.Entity<Order>().HasData(orders);
        }
    }
}
