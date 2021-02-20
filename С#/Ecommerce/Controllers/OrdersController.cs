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
            if (sum > 0)
            {
                var customerOrder =
                    await _context.CustomerInvestments
                    .AsNoTracking()
                    .Where(p => p.Sum > sum)
                    .Select(p => p.CustomerEmail)
                    .Distinct()
                    .ToListAsync();

                return customerOrder;
            }

            return BadRequest();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CustomerOrder>>> GetCustomer(string id)
        {
            if (id == default)
            {
                return BadRequest();
            }

            var customerOrder = 
                await _context.CustomerOrders
                .AsNoTracking()
                .Where(m => m.CustomerEmail == id)
                .ToListAsync();

            return customerOrder;
        }

        // PUT: api/Orders/5       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("");
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

        public async Task<bool> IsValidOrder(Order order)
        {
            var _customer = await _context.Customers.FindAsync(order.Customer.Email);

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

        public async void UpdateProductList(LineBuffer item)
        {
            ProductList productList = await _context.ProductLists
                            .Where(p => p.ProductName == item.ProductName)
                            .FirstOrDefaultAsync();

            if (productList == null)
            {
                productList = new ProductList
                {
                    ProductName = item.ProductName,
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
        }

        public async Task<decimal> GetSum(LineBuffer item)
        {
            Product product = await _context.Products.FindAsync(item.ProductName);

            if (product == null)
            {
                return default;
            }

            return product.Price * item.Quantity;
        }

        // POST: api/Orders       
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            if (!await IsValidOrder(order))
            {
                return BadRequest();
            }

            CustomerOrder customerOrder = new CustomerOrder
            {
                CustomerEmail = order.CustomerEmail,
                OrderNumber = order.Number
            };

            decimal sum = 0;

            List<LineItem> lineItems = new List<LineItem>();

            foreach (LineBuffer item in order.Items)
            {
                UpdateProductList(item);
                lineItems.Add(
                new LineItem
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                });

                Product product = await _context.Products.FindAsync(item.ProductName);
                if (product == null)
                {
                    return NotFound();
                }                

                sum += await GetSum(item);
            }

            if (sum == default)
            {
                return BadRequest();
            }

            order.Items = null;
            customerOrder.Sum = sum;

            var _customerInvestment = await _context.CustomerInvestments
                .Where(p => p.CustomerEmail == order.CustomerEmail).FirstOrDefaultAsync();

            if (_customerInvestment == null)
            {
                await _context.CustomerInvestments.AddAsync
                    (new CustomerInvestment { Sum = sum, 
                        CustomerEmail = order.CustomerEmail});
            }
            else
            {
                _customerInvestment.Sum += sum;
                _context.CustomerInvestments.Update(_customerInvestment);
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

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
