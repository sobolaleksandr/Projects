using domain;
using HallOfFame.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
namespace HallOfFame.Tests
{
    public class SkillsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfPeople()
        {
            // Arrange
            var mockRepo = new Mock<IPeopleRepository>();
            mockRepo.Setup(repo => repo.GetPeople())
                .ReturnsAsync(GetTestSessions());
            var controller = new PeopleController(mockRepo.Object);

            // Act
            var result = await controller.GetPeople();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }
    }
}
