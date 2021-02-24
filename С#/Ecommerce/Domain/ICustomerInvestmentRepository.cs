using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using Ecommerce.ViewModels;
namespace Ecommerce.Domain
{
    public interface ICustomerInvestmentRepository
    {
        Task Create(Order order, decimal sum);
    }
}
