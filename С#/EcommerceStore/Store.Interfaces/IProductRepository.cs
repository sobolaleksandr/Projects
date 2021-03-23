using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IProductRepository
    {
        Task<Product> GetById(int id);

        Task<IEnumerable<Product>> GetAllSortedByPopularity();

        Task<bool> TryToCreate(Product product);

        Task<bool> Update(int id, Product product);

        Task<Product> Delete(int id);

        Task<Product> GetByName(string name);

        Task<List<Product>> GetAll();
    }
}
