using AB.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AB.Domain
{
    public interface IRecordsRepository
    {
        bool ValidateRecords(IEnumerable<tblRecord> records);

        Task<bool> TryToAddRecords(IEnumerable<tblRecord> records);

        Task<int> GetReturned(DateTime day);

        Task<int> GetInstalled(DateTime day);
    }
}
