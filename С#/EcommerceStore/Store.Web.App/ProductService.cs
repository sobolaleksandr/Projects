using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Web.App
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(
            IProductRepository productRepository
            )
        {
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductView>> GetAllByPopularity()
        {
            return await productRepository.GetAllByPopularity();
        }

        public async Task<Product> GetById(int id)
        {
            return await productRepository.GetById(id);
        }

        public async Task<bool> Update(int id, Product product)
        {
            return await productRepository.Update(id, product);
        }

        public async Task<bool> TryToCreate(Product product)
        {
            return await productRepository.TryToCreate(product);
        }

        public async Task<Product> Delete(int id)
        {
            return await productRepository.Delete(id);
        }
    }
}
