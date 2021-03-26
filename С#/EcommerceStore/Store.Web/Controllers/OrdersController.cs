using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Web.App;

namespace Store.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService orderService;

        public OrdersController(OrderService orderService)
        {
            this.orderService = orderService;
        }

        //Список клиентов, заказавших товара на сумму, превышающую указанную
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerInvestmentsView>>> 
            GetCustomersInvestmentsBySum(decimal sum)
        {
            if (sum >= 0)
                return new ObjectResult(await orderService.GetCustomerInvestmentsBySum(sum));

            return BadRequest();
        }

        //Список заказов для указанного клиента, с указанием общей стоимости каждого
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CustomerOrders>>> 
            GetCustomerOrders(string id)
        {
            if (id == default)
                return BadRequest();

            return new ObjectResult(await orderService.GetCustomerOrdersById(id));
        }

        //Оставил реализацию по-умолчанию
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            if (await orderService.Update(id, order))
                return NoContent();

            return NotFound();
        }

        //Создаем заказ, вместе с ним создаем клиента.
        //Если клиента существует, создаем для него заказ
        [HttpPost]
        public async Task<ActionResult<OrderModel>> CreateOrder(OrderModel order)
        {
            if (ModelState.IsValid)
                if (await orderService.TryToCreate(order))
                    return Ok();

            return BadRequest();
        }

        //Оставил реализацию по-умолчанию
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderModel>> DeleteOrder(int id)
        {
            var order = await orderService.Delete(id);

            if (order == null)
                return NotFound();

            return new ObjectResult(order);
        }

    }
}
