using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Customer
    {
        public int id { get; set; }
        public string name { get; set; }
        [Key]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        public string email { get; set; }
        [Required]
        public List<Order> order { get; set; }
    }
}
