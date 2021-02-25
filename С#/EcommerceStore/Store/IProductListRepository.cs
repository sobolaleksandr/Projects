using System.Threading.Tasks;

namespace Store
{
    public interface IProductListRepository
    {
        Task CreateOrUpdate(LineBuffer item);
    }
}
