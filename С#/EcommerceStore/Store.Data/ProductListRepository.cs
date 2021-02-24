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

        public async Task Create(LineBuffer item)
        {
            ProductList productList = await _context.ProductLists
                            .Where(p => p.ProductName == item.ProductName)
                            .FirstOrDefaultAsync();

            if (productList == null)
            {
                productList = new ProductList
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Popularity = 1
                };
                await _context.ProductLists.AddAsync(productList);
            }
            else
            {
                productList.Quantity += item.Quantity;
                productList.Popularity += 1;
                _context.ProductLists.Update(productList);
            }

            await _context.SaveChangesAsync();
        }
    }
}
