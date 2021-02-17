using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class LineItem
    {
        public int id { get; set; }
        public uint quantity { get; set; }
        public Product product { get; set; }
    }
}
