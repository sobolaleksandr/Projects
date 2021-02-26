using Store.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Store.Web.App
{
    public class ProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IProductListRepository productListRepository;

        public ProductService(IProductRepository productRepository,
                              IProductListRepository productListRepository)
        {
            this.productRepository = productRepository;
            this.productListRepository = productListRepository;
        }

        public async Task<IEnumerable<ProductList>> GetAll()
        {
            return await productListRepository.GetAll();
        }

        public async Task<Product> GetById(string id)
        {
            return await productRepository.GetById(id);
        }

        public async Task<bool> Update(string id, Product product)
        {
            return await productRepository.Update(id, product);
        }

        public async Task<bool> TryToCreate(Product product)
        {
            return await productRepository.TryToCreate(product);
        }

        public async Task<Product> Delete(string id)
        {
            return await productRepository.Delete(id);
        }
    }
}
