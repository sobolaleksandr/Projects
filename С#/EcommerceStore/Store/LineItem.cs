﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Store
{
    public class LineItem
    {
        public int Id { get; set; }

        //Количество продуктов в одной позиции - положительное число
        [Range(1, 999999999999, ErrorMessage =
        "Недопустимое значение. Количество должно быть больше нуля")]
        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public int OrderNumber { get; set; }
    }
}
