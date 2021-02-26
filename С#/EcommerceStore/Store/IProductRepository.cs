using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IProductRepository
    {
        Task<Product> GetById(string id);
        Task<bool> TryToCreate(Product product);
        Task<bool> Update(string id, Product product);
        Task<Product> Delete(string id);
    }
}
