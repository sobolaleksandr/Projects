﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Memory;
using Store.Views;
using Store.Web.App;

namespace Store.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductsController(ProductService productService)
        {
            this.productService = productService;
        }

        //Возращаем список продуктов, отсортированных по популярности
        //для кажого продукта указываем общее количество проданных единиц
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductList>>> GetProducts()
        {
            return new ObjectResult(await productService.GetAll());
        }

        //Оставил реализацию по-умолчанию
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            if (id == default)
            {
                BadRequest();
            }

            var product = await productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return new ObjectResult(product);
        }

        //Оставил реализацию по-умолчанию
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, Product product)
        {
            if (id != product.Name || id == default(string))
            {
                return BadRequest();
            }

            if (await productService.Update(id, product))
            {
                return NoContent();
            }

            return NotFound();
        }

        //Оставил реализацию по-умолчанию
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                if (await productService.TryToCreate(product))
                {
                    return CreatedAtAction(nameof(GetProduct), 
                        new { id = product.Name }, product);
                }

                return Conflict();
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

            var product = await productService.Delete(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
    }
}
