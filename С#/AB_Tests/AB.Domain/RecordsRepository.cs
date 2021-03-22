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
        private readonly List<Domain.Models.Record> dbContext = new()
        {
            new Domain.Models.Record
            {
                Id = Guid.NewGuid().ToString(),
                RegistrationDate = new DateTime(1999, 10, 5),
                LastActivityDate = new DateTime(1999, 10, 25)
            },
            new Domain.Models.Record
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

        public async Task<bool> TryToAddRecords(IEnumerable<Record> records)
        {
            foreach (var record in records)
                if (!(await TryToAddRecord(record)))
                    return false;

            return true;
        }

        public Task<bool> TryToAddRecord(Record record)
        {
            if (record.LastActivityDate<record.RegistrationDate)
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
