using System;
using System.ComponentModel.DataAnnotations;

namespace Store
{
    //Временный буфер для хранения "оригинальных" товаров
    public class LineBuffer
    {
        public int Id { get; set; }
        //Количество продуктов в одной позиции - положительное число
        [Range(1, 999999999999, ErrorMessage =
        "Недопустимое значение. Количество должно быть больше нуля")]
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
}


