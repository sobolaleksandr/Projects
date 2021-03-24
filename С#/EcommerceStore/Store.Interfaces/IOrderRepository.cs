using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store
{
    public interface IOrderRepository
    {
        Task<bool> Update(int id, Order order);

        Task<bool> TryToCreate(OrderModel order);

        Task<Order> Delete(int id);

        Task<IEnumerable<CustomerInvestmentsView>> GetCustomersBySum(decimal sum);

        Task<Order> GetByNumber(int number);

        Task<IEnumerable<CustomerOrders>> GetCustomerOrdersById(string id);
    }
}
