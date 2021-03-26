using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Xunit;
using Newtonsoft.Json;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System;
using AB.Domain.Models;

namespace AB.IntegrationTests
{
    public class RecordsTests
    {
        private readonly HttpClient _client;

        readonly List<AB.Domain.Models.tblRecord> records = new()
        {
            new AB.Domain.Models.tblRecord
            {
                Id = Guid.NewGuid().ToString(),
                RegistrationDate = DateTime.Today.AddDays(-1),
                LastActivityDate = DateTime.Today,
            },
            new AB.Domain.Models.tblRecord
            {
                Id = Guid.NewGuid().ToString(),
                RegistrationDate = DateTime.Today.AddDays(-2),
                LastActivityDate = DateTime.Today.AddDays(-1)
            },
        };

        readonly double[] values = new double[]{400d, 200d, 150d, 100d, 100d, 100d, 100d};

        public RecordsTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        internal async Task CreatePerson_WithGoodModel_ReturnsOk()
        {
            // Arrange
            var body = JsonConvert.SerializeObject(records);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Records")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            request = null;
            response = null;

            // Arrange
            request = new HttpRequestMessage(HttpMethod.Get, $"/api/Records");

            // Act
            response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            List<HistogramColumn> jsonHistogram = JsonConvert.DeserializeObject<List<HistogramColumn>>(content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            for(int i=0; i<7; i++)
            {
                Assert.Equal(DateTime.Today.AddDays(-i), jsonHistogram[i].Day);
                Assert.Equal(values[i], jsonHistogram[i].Value);
            }
        }

        [Fact]
        internal async Task CreatePerson_WithBadModel_ReturnsBadRequest()
        {
            // Arrange
            var body = JsonConvert.SerializeObject(new AB.Domain.Models.tblRecord());
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Records")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}