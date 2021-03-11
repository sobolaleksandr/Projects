using Xunit;
using HallOfFame.Web.Controllers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using HallOfFame.Web;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using HallOfFame.Data;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.IntegrationTest
{
    public class PeopleTest
    {
        private readonly HttpClient _client;
        private PersonJson testPerson = new PersonJson();

        public class PersonJson
        {
            [JsonProperty(PropertyName = "Id")]
            public long Id { get; set; }
            [JsonProperty(PropertyName = "Name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "DisplayName")]
            public string DisplayName { get; set; }
            [JsonProperty(PropertyName = "SkillsCollection")]
            public List<SkillJson> SkillsCollection { get; set; }
        }
        public class SkillJson
        {
            [JsonProperty(PropertyName = "Id")]
            public long Id { get; set; }
            [JsonProperty(PropertyName = "Name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "Level")]
            public byte Level { get; set; }
            [JsonProperty(PropertyName = "PersonId")]
            public long PersonId { get; set; }
        }

        public PeopleTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact] 
        public async Task PeopleTests()
        {
            await CreatePerson_WithGoodModel_ReturnsOk();
            await GetPersons_ReturnsPersons();
            await UpdatePerson_WithGoodModel_ReturnsOk();
            await GetPerson_ReturnsPerson();
            await DeletePerson_WithGoodId_ReturnsOk();
        }

        internal async Task CreatePerson_WithGoodModel_ReturnsOk()
        {
            // Arrange
            Person person =
            new Person
            {
                Name = "TestsName",
                SkillsCollection =
                new Skill[]
                {
                    new Skill
                    {
                        Name="TestSkill",
                        Level = 9
                    },
                    new Skill
                    {
                        Name="TestSkill2",
                        Level = 9
                    }
                }
            };

            var body = JsonConvert.SerializeObject(person);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/person/");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task CreatePerson_WithId_ReturnsBadRequest()
        {
            // Arrange
            Person person =
            new Person
            {
                Id=1,
                Name = "TestsName",
                SkillsCollection =
                new Skill[]
                {
                    new Skill
                    {
                        Name="TestSkill",
                        Level = 9
                    },
                    new Skill
                    {
                        Name="TestSkill2",
                        Level = 9
                    }
                }
            };

            var body = JsonConvert.SerializeObject(person);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/person/");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        internal async Task CreatePerson_WithBadModel_ReturnsBadRequest()
        {
            // Arrange
            Person person =
            new Person
            {
                Name = "TestsName",
                SkillsCollection =
                new Skill[]
                {
                    new Skill
                    {
                        Name="TestSkill",
                        Level = 11
                    },
                    new Skill
                    {
                        Name="TestSkill2",
                        Level = 9
                    }
                }
            };

            var body = JsonConvert.SerializeObject(person);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/person/");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        internal async Task CreatePerson_WithNull_ReturnsBadRequest()
        {
            // Arrange
            Person person = null;

            var body = JsonConvert.SerializeObject(person);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/person/");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        internal async Task GetPersons_ReturnsPersons()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/persons");

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            List<PersonJson> jsonPerson = JsonConvert.DeserializeObject<List<PersonJson>>(content);
            testPerson = jsonPerson.Where(p => p.Name == "TestsName").FirstOrDefault();
            // Assert

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        internal async Task UpdatePerson_WithGoodModel_ReturnsOk()
        {
            // Arrange
            testPerson.SkillsCollection[0].Level = 1;
            testPerson.SkillsCollection[1].Level = 1;
            long id = testPerson.Id;

            var body = JsonConvert.SerializeObject(testPerson);
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/person/{id}");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task UpdatePerson_WithNullId_ReturnBadRequest()
        {
            // Arrange
            long? id = testPerson.Id;

            var body = JsonConvert.SerializeObject(testPerson);
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/person/{id}");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        internal async Task UpdatePerson_WithBadId_ReturnBadRequest()
        {
            // Arrange
            long id = 1;
            Person person =
            new Person
            {
                Id = 12,
                Name = "TestsName",
                SkillsCollection =
                new Skill[]
                {
                    new Skill
                    {
                        Name="TestSkill",
                        Level = 9
                    },
                    new Skill
                    {
                        Name="TestSkill2",
                        Level = 9
                    }
                }
            };

            var body = JsonConvert.SerializeObject(testPerson);
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/person/{id}");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        internal async Task UpdatePerson_WithBadModel_ReturnsBadRequest()
        {
            // Arrange
            long id = 1;

            Person person =
                        new Person
                        {
                            Id = id,
                            Name = "TestsName",
                            SkillsCollection =
                            new Skill[]
                            {
                    new Skill
                    {
                        Name="TestSkill",
                        Level = 11
                    },
                    new Skill
                    {
                        Name="TestSkill2",
                        Level = 9
                    }
                            }
                        };

            var body = JsonConvert.SerializeObject(testPerson);
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/person/{id}");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        internal async Task UpdatePerson_WithNonExistingPerson_ReturnsNotFound()
        {
            // Arrange
            long id = 12;

            Person person =
            new Person
            {
                Id = id,
                Name = "TestsName",
                SkillsCollection =
                new Skill[]
                {
                    new Skill
                    {
                        Name="TestSkill",
                        Level = 9
                    },
                    new Skill
                    {
                        Name="TestSkill2",
                        Level = 9
                    }
                }
            };

            var body = JsonConvert.SerializeObject(person);
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/person/{id}");
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        internal async Task GetPerson_ReturnsPerson()
        {
            // Arrange
            long id = testPerson.Id;
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/person/{id}");

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            PersonJson jsonPerson = JsonConvert.DeserializeObject<PersonJson>(content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal("TestsName", jsonPerson.Name);
            Assert.Null(jsonPerson.DisplayName);
            Assert.Equal(2, jsonPerson.SkillsCollection.Count);
            Assert.Equal("TestSkill", jsonPerson.SkillsCollection[0].Name);
            Assert.Equal(1, jsonPerson.SkillsCollection[0].Level);
            Assert.Equal("TestSkill2", jsonPerson.SkillsCollection[1].Name);
            Assert.Equal(1, jsonPerson.SkillsCollection[1].Level);
        }

        [Fact]
        internal async Task GetPerson_ReturnsNotFound()
        {
            // Arrange
            long id = 9999;
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/person/{id}");

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            PersonJson jsonPerson = JsonConvert.DeserializeObject<PersonJson>(content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        internal async Task DeletePerson_WithGoodId_ReturnsOk()
        {
            // Arrange
            long id = testPerson.Id;
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/person/{id}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        internal async Task DeletePerson_WithBadId_ReturnsNotFound()
        {
            // Arrange
            long id = 0;
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/person/{id}");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
