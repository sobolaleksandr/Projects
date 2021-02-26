using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Store.Memory
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<EcommerceContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                },
                ServiceLifetime.Transient
            );

            services.AddScoped<Dictionary<Type, EcommerceContext>>();
            services.AddSingleton<DbContextFactory>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<ICustomerOrderRepository, CustomerOrderRepository>();
            services.AddSingleton<ICustomerInvestmentRepository, CustomerInvestmentRepository>();
            services.AddSingleton<ILineItemsRepository, LineItemsRepository>();
            services.AddSingleton<IProductListRepository, ProductListRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();

            return services;
        }
    }
}