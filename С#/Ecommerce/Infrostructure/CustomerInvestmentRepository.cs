using Ecommerce.Domain;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Infrostructure
{
    public class CustomerInvestmentRepository : ICustomerInvestmentRepository
    {
        private EcommerceContext _context;

        public CustomerInvestmentRepository(EcommerceContext context)
        {
            _context = context;
        }
        public async Task Create(Order order, decimal sum)
        {
            var _customerInvestment = await _context.CustomerInvestments
                .Where(p => p.CustomerEmail == order.CustomerEmail).FirstOrDefaultAsync();

            if (_customerInvestment == null)
            {
                await _context.CustomerInvestments.AddAsync(
                    new CustomerInvestment
                    {
                        Sum = sum,
                        CustomerEmail = order.CustomerEmail
                    });
            }
            else
            {
                _customerInvestment.Sum += sum;
                _context.CustomerInvestments.Update(_customerInvestment);
            }

            await _context.SaveChangesAsync();
        }
    }
}
