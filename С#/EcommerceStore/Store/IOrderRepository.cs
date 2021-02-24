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
        Task<ActionResult<IEnumerable<string>>> GetAll(decimal sum);
        Task<ActionResult<IEnumerable<CustomerOrder>>> Get(string id);
        Task<bool> Put(int id, Order order);
        Task Create(Order order);
        Task<bool> Delete(int id);
        Task<bool> IsValidOrder(Order order);
        Task<decimal> GetSum(LineBuffer item);
    }
}
