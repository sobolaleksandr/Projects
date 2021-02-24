using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public interface ILineItemsRepository
    {
        Task Create(LineBuffer item);
    }
}
