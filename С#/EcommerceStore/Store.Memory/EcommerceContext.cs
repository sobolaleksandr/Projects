using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Store.Memory
{
    public class EcommerceContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<OrderModel> OrderModels { get; set; }

        public EcommerceContext(DbContextOptions<EcommerceContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildProducts(modelBuilder);
            BuildCustomers(modelBuilder);
            BuildOrders(modelBuilder);
        }

        private static void BuildProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(action =>
            {
                action.HasAlternateKey(u => new { u.Name });//Имя продукта уникально

                action.HasData(
                new Product { Id = 1, Name = "mango", Price = 230 },
                new Product { Id = 2, Name = "banana", Price = 206 },
                new Product { Id = 3, Name = "apple", Price = 100 }
                );
            });
        }

        private static void BuildCustomers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(action =>
            {
                action.HasData(
                new Customer { Name = "Tom", Email = "Tomt@yahoo.com" },
                new Customer { Name = "Bob", Email = "bob00@gmail.com" },
                new Customer { Name = "Bill", Email = "Bill2@yandex.ru" }
                );
            });
        }

        private static void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(action =>
            {
                action.HasAlternateKey(u => new { u.Number });//номер заказа уникален

                //action.HasData(
                //new 
                //{
                //    Id = 1,
                //    Created = DateTime.Today,
                //    Number = 1,
                //    CustomerId = 1
                //}
                //,
                //new Order
                //{
                //    Created = DateTime.Today,
                //    Number = 2,
                //    Customer = new Customer { Id = 1, Name = "Tom", Email = "Tomt@yahoo.com" }
                //},
                //new Order
                //{
                //    Created = DateTime.Today,
                //    Number = 3,
                //    Customer = new Customer { Id = 1, Name = "Tom", Email = "Tomt@yahoo.com" }
                //}
                //);
            });
        }
    }

}