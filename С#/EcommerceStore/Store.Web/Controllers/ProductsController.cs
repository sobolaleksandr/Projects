using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Memory;
using Store.Views;

namespace Store.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public ProductsController(EcommerceContext context)
        {
            _context = context;
        }

        //Возращаем список продуктов, отсортированных по популярности
        //для кажого продукта указываем общее количество проданных единиц
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductList>>> GetProducts()
        {
            return await _context.ProductLists
                .AsNoTracking()
                .OrderByDescending(p => p.Popularity)
                .ToListAsync();
        }

        //Оставил реализацию по-умолчанию
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            if (id == default)
            {
                BadRequest();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        //Оставил реализацию по-умолчанию
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            if (id != product.Name || id == default(string))
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //Оставил реализацию по-умолчанию
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (ProductExists(product.Name))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetProduct", new { id = product.Name }, product);
            }

            return BadRequest();
        }

        //Оставил реализацию по-умолчанию
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            if (id == default(string))
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        //Оставил реализацию по-умолчанию
        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.Name == id);
        }
    }
}
