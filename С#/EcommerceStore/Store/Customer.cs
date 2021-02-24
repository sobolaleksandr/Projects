﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store
{
    public class Customer
    {
        public string Name { get; set; }
        [Key]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; }
    }
}
