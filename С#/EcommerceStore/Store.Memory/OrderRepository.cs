using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Memory
{
    public class OrderRepository : IOrderRepository
    {
        private EcommerceContext _context;

        public OrderRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetSum(LineBuffer item)
        {
            Product product = await _context.Products.FindAsync(item.ProductName);

            return product.Price * item.Quantity;
        }

        public async Task<bool> IsValidOrder(Order order)
        {
            Customer customer = await _context.Customers.FindAsync(order.Customer.Email);

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
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerOrder>> Get(string id)
        {
            return await _context.CustomerOrders
                .AsNoTracking()
                .Where(m => m.CustomerEmail == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAll(decimal sum)
        {
            return await _context.CustomerInvestments
                .AsNoTracking()
                .Where(p => p.Sum > sum)
                .Select(p => p.CustomerEmail)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Put(int id, Order order)
        {
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
            return _context.Orders.Any(e => e.Id == id);
        }
    }

}
