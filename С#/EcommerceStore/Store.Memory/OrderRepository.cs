using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Memory
{
    class OrderRepository : IOrderRepository
    {

        private readonly DbContextFactory dbContextFactory;

        public OrderRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }     

        public async Task<Order> GetByNumber(int number)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            return await _context.Orders.Where(o => o.Number == number).FirstOrDefaultAsync();
        }

        public async Task<Order> Delete(int id)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return null;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<bool> Update(int id, Order order)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<Customer[]> GetAllBySum(decimal sum)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            return await _context.Customers.FromSqlRaw(
" Select " +
" Customers.Email from Customers left outer join Orders on " +
" Orders.CustomerEmail = Customers.Email " +
" left outer join LineItems on LineItems.OrderNumber = Orders.Number left outer join " +
" Products on Products.Id = LineItems.ProductId " +
" where Products.Price * LineItems.Quantity > {0} ", sum).ToArrayAsync();
        }

        private async Task CreateItems(OrderModel order)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            foreach (var item in order.Items)
            {
                await _context.LineItems.AddAsync(new LineItem
                {
                    OrderNumber = order.Number,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }
        }

        private async Task CreateCustomer(OrderModel order)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            var customer = await _context.Customers.FindAsync(order.Customer.Email);

            if (customer == null)
                await _context.Customers.AddAsync(order.Customer);
        }

        public async Task<bool> TryToCreate(OrderModel order)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            await CreateItems(order);
            await CreateCustomer(order);

            await _context.Orders.AddAsync(new Order
            {
                Number = order.Number,
                CustomerEmail = order.Customer.Email,
                Created = order.Created
            });

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<object> GetCustomerById(string id)
        {
            throw new System.NotImplementedException();
        }
    }

}
