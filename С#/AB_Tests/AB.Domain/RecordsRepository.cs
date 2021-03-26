using AB.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB.Domain
{
    public class RecordsRepository : IRecordsRepository
    {
        private readonly List<tblRecord> dbContext = new()
        {
            new tblRecord
            {
                Id = Guid.NewGuid().ToString(),
                RegistrationDate = new DateTime(1999, 10, 5),
                LastActivityDate = new DateTime(1999, 10, 25)
            },
            new tblRecord
            {
                Id = Guid.NewGuid().ToString(),
                RegistrationDate = new DateTime(2005, 10, 5),
                LastActivityDate = new DateTime(2005, 10, 5)
            },
        };

        public Task<int> GetInstalled(DateTime day)
        {
            return Task.FromResult(dbContext.Where(d => d.LastActivityDate >= day)
                             .Count());
        }

        public Task<int> GetReturned(DateTime day)
        {
            return Task.FromResult(dbContext.Where(d => d.RegistrationDate <= day)
                            .Count());
        }

        public async Task<bool> TryToAddRecords(IEnumerable<tblRecord> records)
        {
            foreach (var record in records)
                if (!(await TryToAddRecord(record)))
                    return false;

            return true;
        }

        public bool ValidateRecords(IEnumerable<tblRecord> records)
        {
            if (records != null)
                if (records.Any())
                    return true;

            return false;
        }

        public Task<bool> TryToAddRecord(tblRecord record)
        {
            if (record.LastActivityDate < record.RegistrationDate)
                return Task.FromResult(false);

            record.Id = Guid.NewGuid().ToString();

            try
            {
                dbContext.Add(record);
                return Task.FromResult(true);
            }
            catch 
            {
                return Task.FromResult(false);
            }
        }
    }
}
