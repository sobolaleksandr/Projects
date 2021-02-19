using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    //сводная таблица для отображения информации
    //можно добавить имя клиента и др. параметры
    //для задания включил только нужные поля
    public class CustomerOrder
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public string CustomerEmail { get; set; }

        [Range(1, 999999999999)]
        [Column(TypeName = "money")]
        public decimal Sum { get; set; }
    }
}
