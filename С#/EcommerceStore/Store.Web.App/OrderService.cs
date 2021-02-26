using Store.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Web.App
{
    public class OrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerInvestmentRepository customerInvestmentRepository;
        private readonly ILineItemsRepository lineItemsRepository;
        private readonly IProductListRepository productListRepository;
        private readonly ICustomerOrderRepository customerOrderRepository;

        public OrderService(
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

        public async Task<IEnumerable<string>> GetAllBySum(decimal sum)
        {
            return await orderRepository.GetAllBySum(sum);
        }

        public async Task<IEnumerable<CustomerOrder>> GetById(string id)
        {
            return await customerOrderRepository.GetById(id);
        }

        public async Task<bool> Update(int id, Order order)
        {
            return await orderRepository.Update(id, order);
        }

        public async Task<bool> TryToCreate(Order order)
        {
            if (!await orderRepository.IsValidOrder(order))
            {
                return false;
            }

            decimal sum = 0;

            foreach (LineBuffer item in order.Items)
            {
                await productListRepository.CreateOrUpdate(item);
                await lineItemsRepository.Create(item);
                sum += await orderRepository.GetSum(item);
            }

            if (sum == default)
            {
                return false;
            }

            order.Items = null;

            await customerOrderRepository.Create(order, sum);
            await customerInvestmentRepository.CreateOrUpdate(order, sum);
            await orderRepository.Create(order);

            return true;
        }

        public async Task<Order> Delete(int id)
        {
            return await orderRepository.Delete(id);
        }

    }

}
