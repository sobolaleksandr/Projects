using Ecommerce.Domain;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Infrostructure
{
    public class CustomerOrderRepository : ICustomerOrderRepository
    {
        private EcommerceContext _context;
        public CustomerOrderRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task Create(Order order, decimal sum)
        {
            await _context.CustomerOrders.AddAsync(
                new CustomerOrder
                {
                    CustomerEmail = order.CustomerEmail,
                    OrderNumber = order.Number,
                    Sum = sum
                });

            await _context.SaveChangesAsync();
        }
    }
}
