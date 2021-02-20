using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using Ecommerce.ViewModels;

namespace Ecommerce
{
    interface IRepository
    {
        Task<IEnumerable<string>> GetAll(decimal sum);
        Task<IEnumerable<CustomerOrder>> Get(string id);
        Task<bool> Put(int id, Order order);
        Task<IActionResult> Create(Order order);
        Task<bool> Delete(int id);
    }
}
