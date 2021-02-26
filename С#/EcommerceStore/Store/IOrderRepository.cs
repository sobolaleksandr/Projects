using Microsoft.AspNetCore.Mvc;
using Store.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store
{
    public interface IOrderRepository
    {
        Task<IEnumerable<string>> GetAllBySum(decimal sum);
        Task<bool> Update(int id, Order order);
        Task Create(Order order);
        Task<Order> Delete(int id);
        Task<bool> IsValidOrder(Order order);
        Task<decimal> GetSum(LineBuffer item);
    }
}
