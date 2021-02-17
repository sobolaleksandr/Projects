using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Models
{
    public class OrderLineItem
    {
        public int ProductQuantity { get; set; }
        public string ProductName { get; set; }
    }
}
