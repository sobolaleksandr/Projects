using System.Threading.Tasks;

namespace Store
{
    public interface ILineItemsRepository
    {
        Task Create(LineBuffer item);
    }
}
