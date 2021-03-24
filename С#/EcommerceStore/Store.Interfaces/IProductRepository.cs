using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IProductRepository
    {
        Task<Product> GetById(int id);

        Task<IEnumerable<ProductView>> GetAllByPopularity();

        Task<bool> TryToCreate(Product product);

        Task<bool> Update(int id, Product product);

        Task<Product> Delete(int id);

        Task<List<Product>> GetAll();
    }
}
