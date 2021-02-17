using System.Collections.Generic;


namespace Catalog.Models
{
    public class CustomerOrder
    {
        public int OrderId { get; set; }
        public string OrderPlaced { get; set; }
        public string OrderFulfilled { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<OrderLineItem> OrderLineItems { get; set; }
    }
}
