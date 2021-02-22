using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store
{
    public interface IBookRepository
    {
        Book[] GetAllByTitle(string titlePart);
    }
}
