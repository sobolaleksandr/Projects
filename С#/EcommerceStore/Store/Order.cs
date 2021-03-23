using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store
{
    public class Order
    {
        public int Id { get; set; }

        public int Number { get; set; }

        //[DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public string CustomerEmail { get; set; }
    }
}
