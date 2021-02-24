using System.Threading.Tasks;

namespace Store
{
    public interface ICustomerInvestmentRepository
    {
        Task Create(Order order, decimal sum);
    }
}
