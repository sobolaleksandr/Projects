using Microsoft.EntityFrameworkCore;
using Store.Memory;
using Store.Views;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Data
{
    public class CustomerInvestmentRepository : ICustomerInvestmentRepository
    {
        private EcommerceContext _context;

        public CustomerInvestmentRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task CreateOrUpdate(Order order, decimal sum)
        {
            CustomerInvestment customerInvestment = await GetByEmail(order.CustomerEmail);

            if (customerInvestment == null)
                await Add(order.CustomerEmail, sum);
            else
                Update(customerInvestment, sum);

            await _context.SaveChangesAsync();
        }

        private async Task<CustomerInvestment> GetByEmail(string Email)
        {
            return await _context.CustomerInvestments
                            .Where(p => p.CustomerEmail == Email)
                            .FirstOrDefaultAsync();
        }

        private async Task Add(string Email, decimal sum)
        {
            await _context.CustomerInvestments.AddAsync(
                   new CustomerInvestment
                   {
                       Sum = sum,
                       CustomerEmail = Email
                   });
        }

        private void Update(CustomerInvestment customerInvestment, decimal sum)
        {
            customerInvestment.Sum += sum;
            _context.CustomerInvestments.Update(customerInvestment);
        }

    }

}
