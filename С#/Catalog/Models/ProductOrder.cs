using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Models
{
    public partial class ProductOrder
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
