using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.ViewModels;

namespace Ecommerce
{
    public class EcommerceRepository:IRepository
    {
        private async Task<bool> IsValidOrder(Order order)
        {
            Customer _customer = await _context.Customers.FindAsync(order.Customer.Email);

            if (_customer != null)
            {
                order.Customer = null;
            }

            try
            {
                await _context.LineBuffers.AddRangeAsync(order.Items);
                _context.LineBuffers.RemoveRange(order.Items);
            }
            catch
            {
                return false;
            }

            return true;
        }
        private EcommerceContext _context;
        public EcommerceRepository(EcommerceContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create(Order order)
        {
            return null;
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
            if (id != order.Id)
            {
                return false;
            }

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
