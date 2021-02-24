using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store
{
    public class Product
    {
        [Key]
        public string Name { get; set; }
        [Column(TypeName = "money")]
        //Цена продукта - неотрицательное число
        [Range(0.01, 999999999999, ErrorMessage =
        "Недопустимое значение. Цена должна быть больше нуля")]
        public decimal Price { get; set; }
    }
}
