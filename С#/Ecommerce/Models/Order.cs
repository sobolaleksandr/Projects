using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Order
    {
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        public int Id { get; set; }
        public int Number { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "Некорректный адрес")]
        public string CustomerEmail { get; set; }
        public Customer Customer { get; set; }
        public List<LineItem> Items { get; set; }//Несколько позиций с продуктами
    }
}
