using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store;
using Store.Views;

namespace Store.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerInvestmentRepository customerInvestmentRepository;
        private readonly ILineItemsRepository lineItemsRepository;
        private readonly IProductListRepository productListRepository;
        private readonly ICustomerOrderRepository customerOrderRepository;

        public OrdersController(
        IOrderRepository orderRepository,
        ICustomerInvestmentRepository customerInvestmentRepository,
        ILineItemsRepository lineItemsRepository,
        IProductListRepository productListRepository,
        ICustomerOrderRepository customerOrderRepository)
        {
            this.orderRepository = orderRepository;
            this.customerInvestmentRepository = customerInvestmentRepository;
            this.lineItemsRepository = lineItemsRepository;
            this.productListRepository = productListRepository;
            this.customerOrderRepository = customerOrderRepository;
        }

        // GET: api/Orders
        [HttpGet]
        //Список клиентов, заказавших товара на сумму, превышающую указанную
        public async Task<ActionResult<IEnumerable<string>>> GetCustomers(decimal sum)
        {
            if (sum > 0)
            {
                return await orderRepository.GetAll(sum);
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

            return await orderRepository.Get(id);
        }

        // PUT: api/Orders/5       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("");
            }
            if (await orderRepository.Put(id, order))
            {
                return NoContent();
            }

            return NotFound();
        }


        // POST: api/Orders       
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            if (!await orderRepository.IsValidOrder(order))
            {
                return BadRequest();
            }

            decimal sum = 0;

            foreach (LineBuffer item in order.Items)
            {
                await productListRepository.Create(item);
                await lineItemsRepository.Create(item);
                sum += await orderRepository.GetSum(item);
            }

            if (sum == default)
            {
                return BadRequest();
            }

            order.Items = null;

            await customerOrderRepository.Create(order, sum);
            await customerInvestmentRepository.Create(order, sum);
            await orderRepository.Create(order);

            return Ok();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            if (await orderRepository.Delete(id))
            {
                return NoContent();
            }

            return NotFound();
        }

    }
}
