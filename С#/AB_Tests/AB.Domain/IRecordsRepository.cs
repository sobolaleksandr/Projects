using AB.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AB.Domain
{
    public interface IRecordsRepository
    {
        Task<bool> TryToAddRecords(IEnumerable<Record> records);

        Task<int> GetReturned(DateTime day);

        Task<int> GetInstalled(DateTime day);
    }
}
