using AB.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AB.Services.Tests
{
    public class RecordsServiceTests
    {
        readonly List<Domain.Models.Record> records = new()
        {
                new Domain.Models.Record {
                    Id = Guid.NewGuid().ToString(),
                    RegistrationDate=new DateTime(1999,10,5),
                    LastActivityDate=new DateTime(1999,10,25) },
                new Domain.Models.Record {
                    Id = Guid.NewGuid().ToString(),
                    RegistrationDate=new DateTime(2005,10,5),
                    LastActivityDate=new DateTime(2005,10,5) },
        };

        [Fact]
        public async Task TryToAddRecords_WithRecords_ReturnsFalse()
        {
            // Arrange
            var mock = new Mock<IRecordsRepository>();
            mock.Setup(repo => repo.TryToAddRecords(records))
                .ReturnsAsync(false)
                .Verifiable();

            var service = new RecordsService(mock.Object);

            // Act
            var result = await service.TryToAddRecords(records);

            // Assert
            Assert.False(result);
            mock.Verify();
        }

        [Fact]
        public async Task TryToAddRecords_WithRecords_ReturnsTrue()
        {
            // Arrange
            var mock = new Mock<IRecordsRepository>();
            mock.Setup(repo => repo.TryToAddRecords(records))
                .ReturnsAsync(true)
                .Verifiable();

            var service = new RecordsService(mock.Object);

            // Act
            var result = await service.TryToAddRecords(records);

            // Assert
            Assert.True(result);
            mock.Verify();
        }

        [Fact]
        public async Task Calculate_WithDays_ReturnsList()
        {
            // Arrange
            var mock = new Mock<IRecordsRepository>();
            mock.Setup(repo => 
                repo.GetInstalled(DateTime.Today))
                .ReturnsAsync(7)
                .Verifiable();

            mock.Setup(repo =>
                repo.GetReturned(DateTime.Today))
                .ReturnsAsync(3)
                .Verifiable();


            var service = new RecordsService(mock.Object);

            // Act
            var result = await service.Calculate(7);
            var list = result.ToList();

            // Assert
            Assert.Equal(7, list.Count);

            Assert.Equal(3d / 7d * 100, list[0].Value);
            Assert.Equal(DateTime.Today, list[0].Day);

            Assert.Equal(0, list[1].Value);
            Assert.Equal(DateTime.Today.AddDays(-1), list[1].Day);
            mock.Verify();
        }
    }
}
