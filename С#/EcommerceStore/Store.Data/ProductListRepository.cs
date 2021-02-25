using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Memory;
using Store.Views;

namespace Store.Data
{
    public class ProductListRepository : IProductListRepository
    {
        private EcommerceContext _context;

        public ProductListRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task CreateOrUpdate(LineBuffer item)
        {
            ProductList productList = await GetByProductName(item.ProductName);

            if (productList == null)
                await Add(item.ProductName, item.Quantity);
            else
                Update(productList, item.Quantity);

            await _context.SaveChangesAsync();
        }

        private async Task<ProductList> GetByProductName(string ProductName)
        {
            return await _context.ProductLists
                            .Where(p => p.ProductName == ProductName)
                            .FirstOrDefaultAsync();
        }

        private async Task Add(string ProductName, int Quantity)
        {
            await _context.ProductLists.AddAsync(new ProductList
            {
                ProductName = ProductName,
                Quantity = Quantity,
                Popularity = 1
            });
        }

        private void Update(ProductList productList, int Quantity)
        {
            productList.Quantity += Quantity;
            productList.Popularity += 1;
            _context.ProductLists.Update(productList);
        }
    }
}
