using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public string name { get; set; }
        public uint price { get; set; }
    }
}
