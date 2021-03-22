using AB.Domain;
using AB.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AB.Services
{
    public class RecordsService
    {
        private readonly IRecordsRepository recordsRepository;
        public RecordsService(
            IRecordsRepository recordsRepository
            )
        {
            this.recordsRepository = recordsRepository;
        }

        public async Task<bool> TryToAddRecords(IEnumerable<Record> records)
        {
            if (await recordsRepository.TryToAddRecords(records))
                return true;

            return false;
        }

        public async Task<IEnumerable<HistogramColumn>> Calculate(int days)
        {
            List<HistogramColumn> histogram = new();

            for (int day = 0; day < days; day++)
            {
                DateTime neededDay = DateTime.Today.AddDays(-day);
                int recordsReturned = await recordsRepository.GetReturned(neededDay);
                int recordsInstalled = await recordsRepository.GetInstalled(neededDay);

                histogram.Add(new HistogramColumn(recordsReturned, recordsInstalled, neededDay));
            }

            return histogram;
        }
    }
}
