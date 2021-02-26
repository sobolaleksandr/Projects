using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Views;
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

        public async Task<decimal> GetSum(LineBuffer item)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            Product product = await _context.Products.FindAsync(item.ProductName);

            return product.Price * item.Quantity;
        }

        public async Task<bool> IsValidOrder(Order order)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            Customer customer = await _context.Customers.FindAsync(order.Customer.Email);

            //сверяем id клиента с его id в заказе
            if (order.Customer.Email != order.CustomerEmail)
            {
                return false;
            }

            //проверяем существует ли клиент
            if (customer != null)
            {
                order.Customer = null;
            }

            //проверяем пустой ли список
            if (order.Items == null)
            {
                return false;
            }

            //проверяем пустой ли список
            if (order.Items.Count == 0)
            {
                return false;
            }

            //проверяем все ли позиции уникальны в заказе
            try
            {
                await _context.LineBuffers.AddRangeAsync(order.Items);
                _context.LineBuffers.RemoveRange(order.Items);
            }
            catch
            {
                return false;
            }

            //проверяем есть ли в БД такие продукты
            foreach (var item in order.Items)
            {
                Product product = await _context.Products.FindAsync(item.ProductName);
                if (product == null)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task Create(Order order)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }



        public async Task<IEnumerable<string>> GetAllBySum(decimal sum)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            return await _context.CustomerInvestments
                .AsNoTracking()
                .Where(p => p.Sum > sum)
                .Select(p => p.CustomerEmail)
                .Distinct()
                .ToListAsync();
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
                if (!Exists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
        private bool Exists(int id)
        {
            var _context = dbContextFactory.Create(typeof(OrderRepository));

            return _context.Orders.Any(e => e.Id == id);
        }
    }

}
