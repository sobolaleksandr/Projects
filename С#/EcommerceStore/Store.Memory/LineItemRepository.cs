using System.Threading.Tasks;

namespace Store.Memory
{
    class LineItemsRepository : ILineItemsRepository
    {

        private readonly DbContextFactory dbContextFactory;

        public LineItemsRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task Create(LineBuffer item)
        {
            var _context = dbContextFactory.Create(typeof(LineItemsRepository));

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
