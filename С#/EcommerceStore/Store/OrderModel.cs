using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store
{
    public class OrderModel
    {
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public Customer Customer { get; set; }

        [Required]
        public IEnumerable<LineItemModel> Items { get; set; }
    }
}
