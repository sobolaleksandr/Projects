using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store
{
    public class LineItemModel
    {
        [Required]
        //Количество продуктов в одной позиции - положительное число
        [Range(1, 999999999999, ErrorMessage =
        "Недопустимое значение. Количество должно быть больше нуля")]
        public int Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
