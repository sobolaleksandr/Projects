using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [Required]
        public string CustomerEmail { get; set; }
    }
}
