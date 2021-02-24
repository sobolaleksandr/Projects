using System.Threading.Tasks;

namespace Store
{
    public interface ICustomerOrderRepository
    {
        Task Create(Order order, decimal sum);
    }
}
