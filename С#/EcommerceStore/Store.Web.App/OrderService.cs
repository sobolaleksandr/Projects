using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Web.App
{
    public class OrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;


        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository
            )
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<CustomerInvestmentsView>> GetCustomerInvestmentsBySum(decimal sum)
        {
            return await orderRepository.GetCustomersBySum(sum);
        }

        public async Task<bool> Update(int id, Order order)
        {
            return await orderRepository.Update(id, order);
        }

        public async Task<Order> Delete(int id)
        {
            return await orderRepository.Delete(id);
        }

        public async Task<bool> TryToCreate(OrderModel order)
        {
            //проверяем все ли позиции уникальны в заказе
            //проверяем есть ли в БД такие продукты
            if (!await ValidateItems(order))
                return false;

            // Проверяем есть ли заказ в базе с таким номером
            if (await orderRepository.GetByNumber(order.Number) != null)
                return false;

            if (order.Created == default)
                order.Created = DateTime.Today;

            if (await orderRepository.TryToCreate(order))
                return true;

            return false;
        }

        public async Task<bool> ValidateItems(OrderModel order)
        {
            if (order.Items.ToList().Count == 0)
                return false;

            List<Product> productList = await productRepository.GetAll();

            foreach (var item in order.Items)
            {
                var product = productList.Where(i => i.Id == item.ProductId).FirstOrDefault();

                if (product == null)
                    return false;

                productList.Remove(product);
            }

            return true;
        }

        public async Task<IEnumerable<CustomerOrders>> GetCustomerOrdersById(string id)
        {
            return await orderRepository.GetCustomerOrdersById(id);
        }
    }

}
