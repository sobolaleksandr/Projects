using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public interface ICustomerOrderRepository
    {
        Task Create(Order order, decimal sum);
    }
}
