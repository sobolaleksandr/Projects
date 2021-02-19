using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using Ecommerce.ViewModels;

namespace Ecommerce.Controllers
{
    [Produces("application/json")]
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
        //Список клиентов, заказавших товара на сумму, превышающую указанную
        public async Task<ActionResult<IEnumerable<string>>> GetCustomers(decimal sum)
        {
            var customerOrder =
                await _context.CustomerInvestments
                .AsNoTracking()
                .Where(p=>p.Sum > sum)
                .Select(p => p.CustomerEmail)
                .Distinct()
                .ToListAsync();

            return customerOrder;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CustomerOrder>>> GetCustomer(string id)
        {
            var customerOrder = 
                await _context.CustomerOrders
                .AsNoTracking()
                .Where(m => m.CustomerEmail == id)
                .ToListAsync();

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

            List<LineItem> lineItems = new List<LineItem>();

            foreach (LineBuffer item in order.Items)
            {
                LineItem lineItem = 
                    new LineItem { ProductName = item.ProductName, Quantity = item.Quantity};
                lineItems.Add(lineItem);

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

            var _customerInvestment = await _context.CustomerInvestments
                .Where(p => p.CustomerEmail == customer.Email).FirstOrDefaultAsync();

            if (_customerInvestment == null)
            {
                await _context.CustomerInvestments.AddAsync
                    (new CustomerInvestment { Sum = sum, CustomerEmail = customer.Email});
            }
            else
            {
                _customerInvestment.Sum += sum;
                _context.CustomerInvestments.Update(_customerInvestment);
            }

            var _customer = await _context.Customers.FindAsync(customer.Email);

            if (_customer != null)
            {
                order.Customer = null;
            }

            await _context.LineItems.AddRangeAsync(lineItems);
            await _context.Orders.AddAsync(order);
            await _context.CustomerOrders.AddAsync(customerOrder);
            await _context.SaveChangesAsync();

            return Ok();
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
