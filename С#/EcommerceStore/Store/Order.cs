using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store
{
    public class Order
    {
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        public int Id { get; set; }
        public int Number { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "Некорректный адрес")]
        public string CustomerEmail { get; set; }
        public Customer Customer { get; set; }
        //Несколько позиций с продуктами
        public ICollection<LineBuffer> Items { get; set; }
    }
}
