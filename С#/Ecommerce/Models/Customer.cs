using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Customer
    {
        public string Name { get; set; }
        [Key]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", 
        ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
