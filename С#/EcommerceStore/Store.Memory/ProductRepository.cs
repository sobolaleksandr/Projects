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

        public async Task<Product> GetByName(string name)
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            return await _context.Products.Where(p => p.Name == name).FirstOrDefaultAsync();
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

        public async Task<IEnumerable<Product>> GetAllSortedByPopularity()
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            string sqlQuery =
" select Products.Name, count(*) as Count, " +
" sum(LineItems.Quantity) as Quantity from Products "+
" LEFT OUTER JOIN "+
" LineItems on Products.Id = LineItems.ProductId "+
" group by Products.name "+
" order by count(*) desc "
                ;
            var result = await _context.Products.FromSqlRaw(sqlQuery)
                                          .ToListAsync();

            return await _context.Products.FromSqlRaw(sqlQuery)
                                          .ToListAsync();
        }

        public async Task<List<Product>> GetAll()
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));
            
            return await _context.Products.ToListAsync();
        }
    }
}
