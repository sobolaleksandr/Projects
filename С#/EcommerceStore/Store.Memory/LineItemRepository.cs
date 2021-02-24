using System.Threading.Tasks;

namespace Store.Memory
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
