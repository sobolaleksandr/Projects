using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class LineItem
    {
        public int Id { get; set; }
        //Количество продуктов в одной позиции - положительное число
        [Range(1, 999999999999, ErrorMessage =
        "Недопустимое значение. Количество должно быть больше нуля")]
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
}
