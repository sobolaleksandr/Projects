using HallOfFame.Data;
using HallOfFame.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HallOfFame.Tests
{


    public class PeopleRepositoryTests
    {
        Person newPerson = new Person
        {
            Name = "",
            SkillsCollection =
                new Skill[]
                {
                    new Skill
                    {
                        Name="agility",
                        Level = 9
                    },
                    new Skill
                    {
                        Name="strength",
                        Level = 9
                    }
                }
        };
        [Fact]
        public async Task AddEmptyPersonReturnsBadRequest()
        {
            // Arrange
            var mock = new Mock<IPeopleRepository>();
            var controller = new PeopleController(mock.Object);
            Person newPerson = new Person();

            // Act
            var result = await controller.CreatePerson(newPerson);

            // Assert
            var viewResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, viewResult?.StatusCode);
        }
    }
}
