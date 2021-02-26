using Store.Memory;
using Store.Views;
using System.Threading.Tasks;

namespace Store.Memory
{
    class CustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public CustomerOrderRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }


        public async Task Create(Order order, decimal sum)
        {
            var _context = dbContextFactory.Create(typeof(CustomerOrderRepository));

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
