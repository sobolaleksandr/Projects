using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Order
    {
        [DataType(DataType.Date)]
        public DateTime created { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int number { get; set; }
        [Required]
        public List<LineItem> lineItem { get; set; }
    }
}
