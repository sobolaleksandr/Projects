using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public OrdersController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerOrder>>> GetCustomer(decimal sum)
        {
            var customerOrder =
                await _context.CustomerOrders.Where(m => m.Sum > sum).ToListAsync();

            //вывожу всю информацию, а не только список клиентов
            //в задании не делается акцент, что это должен быть только список клиентов
            return customerOrder;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CustomerOrder>>> GetCustomer(string id)
        {
            var customerOrder = 
                await _context.CustomerOrders.Where(m => m.CustomerEmail == id).ToListAsync();

            return customerOrder;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> Create(Order order)
        {
            Customer customer = order.Customer;

            CustomerOrder customerOrder = new CustomerOrder();
            customerOrder.CustomerEmail = customer.Email;
            customerOrder.OrderNumber = order.Number;
            decimal sum = 0;

            foreach (LineItem item in order.Items)
            {
                Product product = await _context.Products.FindAsync(item.ProductName);
                if (product == null)
                {
                    return NotFound();
                }
                ProductList productList = await _context.ProductLists
                    .Where(p => p.ProductName == item.ProductName).FirstOrDefaultAsync();

                if (productList == null)
                {
                    productList = new ProductList
                    {
                        ProductName = product.Name,
                        Quantity = item.Quantity,
                        Popularity = 1
                    };
                    await _context.ProductLists.AddAsync(productList);
                }
                else
                {
                    productList.Quantity += item.Quantity;
                    productList.Popularity += 1;
                    _context.ProductLists.Update(productList);
                }

                sum += product.Price * item.Quantity;
            }

            order.Items = null;
            customerOrder.Sum = sum;

            var _customer = await _context.Customers.FindAsync(customer.Email);
            if (_customer != null)
            {
                order.Customer = null;
            }

            await _context.Orders.AddAsync(order);
            await _context.CustomerOrders.AddAsync(customerOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
