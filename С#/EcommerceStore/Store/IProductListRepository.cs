using System.Threading.Tasks;

namespace Store
{
    public interface IProductListRepository
    {
        Task Create(LineBuffer item);
    }
}
