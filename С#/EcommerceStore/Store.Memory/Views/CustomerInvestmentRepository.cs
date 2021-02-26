using Microsoft.EntityFrameworkCore;
using Store.Memory;
using Store.Views;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Memory
{
    class CustomerInvestmentRepository : ICustomerInvestmentRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public CustomerInvestmentRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task CreateOrUpdate(Order order, decimal sum)
        {
            var _context = dbContextFactory.Create(typeof(CustomerInvestmentRepository));

            CustomerInvestment customerInvestment = await GetByEmail(order.CustomerEmail);

            if (customerInvestment == null)
                await Add(order.CustomerEmail, sum);
            else
                Update(customerInvestment, sum);

            await _context.SaveChangesAsync();
        }

        private async Task<CustomerInvestment> GetByEmail(string Email)
        {
            var _context = dbContextFactory.Create(typeof(CustomerInvestmentRepository));

            return await _context.CustomerInvestments
                            .Where(p => p.CustomerEmail == Email)
                            .FirstOrDefaultAsync();
        }

        private async Task Add(string Email, decimal sum)
        {
            var _context = dbContextFactory.Create(typeof(CustomerInvestmentRepository));

            await _context.CustomerInvestments.AddAsync(
                   new CustomerInvestment
                   {
                       Sum = sum,
                       CustomerEmail = Email
                   });
        }

        private void Update(CustomerInvestment customerInvestment, decimal sum)
        {
            var _context = dbContextFactory.Create(typeof(CustomerInvestmentRepository));

            customerInvestment.Sum += sum;
            _context.CustomerInvestments.Update(customerInvestment);
        }

    }

}
