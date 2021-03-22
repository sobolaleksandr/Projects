using AB.Domain.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace AB.Domain.Tests
{
    public class RecordsRepositoryTests
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

        [Fact]
        public async void GetInstalled_WithGoodDay_ReturnsNumber()
        {
            RecordsRepository repo = new();

            int result = await repo.GetInstalled(dbContext[0].RegistrationDate);

            Assert.Equal(2, result);
        }

        [Fact]
        public async void GetInstalled_WithBadDay_ReturnsZero()
        {
            RecordsRepository repo = new();

            int result = await repo.GetInstalled(DateTime.Today);

            Assert.Equal(0, result);
        }

        [Fact]
        public async void GetReturned_WithGoodDay_ReturnsNumber()
        {
            RecordsRepository repo = new();

            int result = await repo.GetReturned(dbContext[0].RegistrationDate);

            Assert.Equal(1, result);
        }

        [Fact]
        public async void GetReturned_WithBadDay_ReturnsZero()
        {
            RecordsRepository repo = new();

            int result = await repo.GetReturned(dbContext[0].RegistrationDate.AddYears(-1));

            Assert.Equal(0, result);
        }

        [Fact]
        public async void TryToAddRecord_WithBadDay_ReturnsFalse()
        {
            RecordsRepository repo = new();

            bool result = await repo.TryToAddRecord(new Domain.Models.Record
            {
                Id = Guid.NewGuid().ToString(),
                RegistrationDate = new DateTime(1999, 10, 26),
                LastActivityDate = new DateTime(1999, 10, 25)
            });

            Assert.False(result);
        }

        [Fact]
        public async void TryToAddRecord_WithGoodDay_ReturnsTrue()
        {
            RecordsRepository repo = new();

            bool result = await repo.TryToAddRecord(new Domain.Models.Record
            {
                Id = Guid.NewGuid().ToString(),
                RegistrationDate = new DateTime(1999, 10, 20),
                LastActivityDate = new DateTime(1999, 10, 25)
            });

            Assert.True(result);
        }
    }
}
