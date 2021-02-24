using Ecommerce.Domain;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Infrostructure
{
    public class LineItemsRepository : ILineItemsRepository
    {

        private EcommerceContext _context;
        public LineItemsRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task Create(LineBuffer item)
        {
            await _context.LineItems.AddAsync(
                new LineItem
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                });

            await _context.SaveChangesAsync();
        }
    }
}
