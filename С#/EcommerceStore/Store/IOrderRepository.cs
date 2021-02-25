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
        Task<IEnumerable<string>> GetAll(decimal sum);
        Task<IEnumerable<CustomerOrder>> Get(string id);
        Task<bool> Put(int id, Order order);
        Task Create(Order order);
        Task<bool> Delete(int id);
        Task<bool> IsValidOrder(Order order);
        Task<decimal> GetSum(LineBuffer item);
    }
}
