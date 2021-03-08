using Microsoft.AspNetCore.Mvc;
using UnitTestApp.Controllers;
using UnitTestApp.Models;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System;
using Xunit.Abstractions;
using System.Threading;

namespace UnitTestApp.Tests
{
    public class HomeControllerTests
    {
        static Mutex mutexObj = new Mutex();
        private readonly ITestOutputHelper output;

        public HomeControllerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void IndexReturnsAViewResultWithAListOfUsers()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestUsers());
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(
                viewResult.Model);
            Assert.Equal(GetTestUsers().Count, model.Count());
        }
        private List<User> GetTestUsers()
        {
            int MaxValue = 100;
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;

            string[] malePetNames = { "Rufus", "Bear", "Dakota", "Fido",
                          "Vanya", "Samuel", "Koani", "Volodya",
                          "Prince", "Yiska" };
            string[] femalePetNames = { "Maggie", "Penny", "Saya", "Princess",
                            "Abby", "Laila", "Sadie", "Olivia",
                            "Starlight", "Talla" };

            var users = new List<User>();

            for (int i = 1; i <= 50; i++)
            {
                var rnd = new Random(System.Convert.ToInt32(DateTime.Now.Millisecond));
                int mIndex = rnd.Next(malePetNames.Length);
                int fIndex = rnd.Next(femalePetNames.Length);
                users.Add(
                    new User 
                { 
                    Id = i,
                    Name = malePetNames[mIndex], 
                    BirthDay = start.AddDays(rnd.Next(range)), 
                    Surname = femalePetNames[fIndex], SecondName = malePetNames[mIndex] + femalePetNames[fIndex], 
                    Balance = rnd.Next(MaxValue) + 1
                });
             }   

            return users;
        }
        [Fact]
        public void AddUserReturnsViewResultWithUserModel()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            User newUser = new User();

            // Act
            var result = controller.AddUser(newUser);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newUser, viewResult?.Model);
        }

        [Fact]
        public void AddUserReturnsARedirectAndAddsUser()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);
            var newUser = new User()
            {
                Name = "Ben"
            };

            // Act
            var result = controller.AddUser(newUser);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.Create(newUser));
        }

        [Fact]
        public void GetUserReturnsBadRequestResultWhenIdIsNull()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.GetUser(null);

            // Arrange
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetUserReturnsNotFoundResultWhenUserNotFound()
        {
            // Arrange
            int testUserId = 1;
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.Get(testUserId))
                .Returns(null as User);
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.GetUser(testUserId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetUserReturnsViewResultWithDecimal()
        {          
            for (int testUserId = 1; testUserId <= 50; testUserId++)
            {
                mutexObj.WaitOne();
                // Arrange
                var mock = new Mock<IRepository>();
                User user = GetTestUsers().FirstOrDefault(p => p.Id == testUserId);
                mock.Setup(repo => repo.Get(testUserId))
                    .Returns(user);
                var controller = new HomeController(mock.Object);

                // Act
                var result = controller.GetUser(testUserId);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<User>(viewResult.ViewData.Model);
                Assert.Equal(user.Balance, model.Balance);
                mutexObj.ReleaseMutex();
            }
        }


        [Fact]
        public void GetUserReturnsViewResultWithDecimalInFlow()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread myThread = new Thread(GetUserReturnsViewResultWithDecimal);
                myThread.Start();
            }
        }
        [Fact]
        public void WithDrawReturnsViewResultWithUser()
        {
            for (int testUserId = 1; testUserId <= 50; testUserId++)
            {
                mutexObj.WaitOne();
                // Arrange
                var rnd = new Random(System.Convert.ToInt32(DateTime.Now.Millisecond));
                decimal sum = rnd.Next(100)-50;

                var mock = new Mock<IRepository>();
                User user = GetTestUsers().FirstOrDefault(p => p.Id == testUserId);
                decimal balance = user.Balance;

                mock.Setup(repo => repo.Get(testUserId))
                    .Returns(user);
                var controller = new HomeController(mock.Object);

                // Act
                var result = controller.Withdraw(testUserId, sum);
                // Assert
                if ((balance + sum) >= 0)
                {
                    balance += sum;
                }
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsType<User>(viewResult.ViewData.Model);
                Assert.Equal(balance, model.Balance);
                mutexObj.ReleaseMutex();
            }
        }
        [Fact]
        public void WithDrawReturnsViewResultWithUserInFlow()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread myThread = new Thread(WithDrawReturnsViewResultWithUser);
                myThread.Start();
            }
        }
    }
}