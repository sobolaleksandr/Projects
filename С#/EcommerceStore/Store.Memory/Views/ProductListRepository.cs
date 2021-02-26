using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Store.Views;

namespace Store.Memory
{
    class ProductListRepository : IProductListRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public ProductListRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<ProductList>> GetAll()
        {
            var _context = dbContextFactory.Create(typeof(ProductListRepository));

            return await _context.ProductLists
                            .AsNoTracking()
                            .OrderByDescending(p => p.Popularity)
                            .ToListAsync();
        }


        public async Task CreateOrUpdate(LineBuffer item)
        {
            var _context = dbContextFactory.Create(typeof(ProductListRepository));

            ProductList productList = await GetByProductName(item.ProductName);

            if (productList == null)
                await Add(item.ProductName, item.Quantity);
            else
                Update(productList, item.Quantity);

            await _context.SaveChangesAsync();
        }

        private async Task<ProductList> GetByProductName(string ProductName)
        {
            var _context = dbContextFactory.Create(typeof(ProductListRepository));

            return await _context.ProductLists
                            .Where(p => p.ProductName == ProductName)
                            .FirstOrDefaultAsync();
        }

        private async Task Add(string ProductName, int Quantity)
        {
            var _context = dbContextFactory.Create(typeof(ProductListRepository));

            await _context.ProductLists.AddAsync(new ProductList
            {
                ProductName = ProductName,
                Quantity = Quantity,
                Popularity = 1
            });
        }

        private void Update(ProductList productList, int Quantity)
        {
            var _context = dbContextFactory.Create(typeof(ProductListRepository));

            productList.Quantity += Quantity;
            productList.Popularity += 1;
            _context.ProductLists.Update(productList);
        }
    }
}
