using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Memory
{
    class ProductRepository : IProductRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public ProductRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<bool> TryToCreate(Product product)
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            _context.Products.Add(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public async Task<Product> Delete(int id)
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return null;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> GetById(int id)
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            return await _context.Products.FindAsync(id);
        }

        public async Task<bool> Update(int id, Product product)
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<ProductView>> GetAllByPopularity()
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            var join = from product in _context.Products
                       join item in _context.LineItems on product.Id equals item.ProductId into gj
                       from sub in gj.DefaultIfEmpty()
                       select new { product.Name, sub.Quantity};

            var result = await join.GroupBy(u => u.Name)
                             .Select(g => new ProductView
                             {
                                 Name = g.Key,
                                 Popularity = g.Count(),
                                 Quantity = g.Sum(c => c.Quantity)
                             })
                             .OrderByDescending(g => g.Popularity)
                             .ThenByDescending(g => g.Quantity)
                             .ToListAsync();

            return result;
        }

        public async Task<List<Product>> GetAll()
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));
            
            return await _context.Products.ToListAsync();
        }
    }
}
