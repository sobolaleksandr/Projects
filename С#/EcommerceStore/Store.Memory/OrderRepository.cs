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

        public async Task<IEnumerable<CustomerInvestmentsView>> GetCustomersBySum(decimal sum)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            var join = from customer in _context.Customers
                       join order in _context.Orders on customer.Email equals order.CustomerEmail
                       join item in _context.LineItems on order.Number equals item.OrderNumber
                       join product in _context.Products on item.ProductId equals product.Id
                       where product.Price * item.Quantity > sum
                       select new CustomerInvestmentsView{ 
                           Email = customer.Email, 
                           Sum = product.Price * item.Quantity };

            return await join.ToListAsync();
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

        public async Task<IEnumerable<CustomerOrders>> GetCustomerOrdersById(string id)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            var join = from order in _context.Orders
                       join item in _context.LineItems on order.Number equals item.OrderNumber
                       join product in _context.Products on item.ProductId equals product.Id
                       where order.CustomerEmail == id
                       select new
                       {
                           order.Number,
                           product.Price,
                           item.Quantity
                       };
            var result = join.GroupBy(o => o.Number)
                                .Select(g => new CustomerOrders
                                {
                                    Number = g.Key,
                                    Sum = g.Sum(g => g.Price * g.Quantity)
                                });

            return await result.ToListAsync();
        }
    }
}
