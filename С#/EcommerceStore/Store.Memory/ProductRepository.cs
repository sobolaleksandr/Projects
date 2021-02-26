using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                if (ProductExists(product.Name))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<Product> Delete(string id)
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> GetById(string id)
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            return await _context.Products.FindAsync(id);
        }



        public async Task<bool> Update(string id, Product product)
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
        private bool ProductExists(string id)
        {
            var _context = dbContextFactory.Create(typeof(ProductRepository));

            return _context.Products.Any(e => e.Name == id);
        }
    }
}
