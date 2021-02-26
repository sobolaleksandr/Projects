using Store.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store
{
    public interface IProductListRepository
    {
        Task CreateOrUpdate(LineBuffer item);

        Task<IEnumerable<ProductList>> GetAll();
    }
}
