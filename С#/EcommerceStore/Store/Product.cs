using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "money")]
        //Цена продукта - неотрицательное число
        [Range(0.01, 999999999999, ErrorMessage =
        "Недопустимое значение. Цена должна быть больше нуля")]
        public decimal Price { get; set; }
    }
}
