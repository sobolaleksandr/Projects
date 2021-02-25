using System.Threading.Tasks;

namespace Store
{
    public interface ICustomerInvestmentRepository
    {
        Task CreateOrUpdate(Order order, decimal sum);
    }
}
