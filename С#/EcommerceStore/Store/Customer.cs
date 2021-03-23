using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store
{
    public class Customer
    {
        public string Name { get; set; }

        //[EmailAddress(ErrorMessage = "Некорректный адрес")]
        [Key]
        public string Email { get; set; }
    }
}
