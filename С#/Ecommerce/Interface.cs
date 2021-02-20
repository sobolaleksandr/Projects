using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce
{
    interface Interface
    {
        //Контроллер Orders
        public class Order
        {
            [DataType(DataType.Date)]
            public DateTime Created { get; set; }
            public int Id { get; set; }
            //Альтернативный ключ
            public int Number { get; set; }
            [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Некорректный адрес")]
            public string CustomerEmail { get; set; }
            public Customer Customer { get; set; }
            public List<LineBuffer> Items { get; set; }//Несколько позиций с продуктами
        }

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

        //Запрашивает клиентов, заказавших товара на сумму, превышающую указанную
        public async Task<ActionResult<IEnumerable<string>>> GetCustomers(decimal sum);
        //Запрашивает список заказов для указанного Клиента с указанием общей стоимости каждого
        //Уникальный ключ типа string
        public async Task<ActionResult<IEnumerable<CustomerOrder>>> GetCustomer(string id);
        //Реализация из шаблона
        public async Task<IActionResult> UpdateCustomer(int id, Order order);
        //Создает заказ, в процессе создает клиента(данные клиента приходят в запрос вместе с описанием заказа)
        //Если заказчик с указанным email-ом существует, создавать заказ для этого клиента
        public async Task<ActionResult<Order>> CreateCustomer(Order order);
        //Реализация из шаблона
        public async Task<ActionResult<Order>> DeleteOrder(int id)

        //Контроллер Products
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
        //Запрашивает список продуктов, отсортированные по популярности (количество уникальных заказов, в котором он участвует),
        //для каждого продукта должно быть указано общее количество проданных единиц
        public async Task<ActionResult<IEnumerable<ProductList>>> GetProduct();
        //Получить продукт по id, уникальный ключ типа string
        public async Task<ActionResult<Product>> GetProduct(string id);
        //Обновить информацию о продукте
        public async Task<IActionResult> UpdateProduct(string id, Product product);
        //Добавить продукт в таблицу
        public async Task<ActionResult<Product>> CreateProduct(Product product);
        //Удалить продукт из таблицы
        public async Task<ActionResult<Product>> DeleteProduct(string id);

    }
}
