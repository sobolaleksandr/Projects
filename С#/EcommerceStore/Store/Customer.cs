using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store
{
    public class Customer
    {
        public string Name { get; set; }

        [Key]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
    }
}
