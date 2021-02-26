using Store.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store
{
    public interface ICustomerOrderRepository
    {
        Task Create(Order order, decimal sum);
        Task<IEnumerable<CustomerOrder>> GetById(string id);
    }
}
