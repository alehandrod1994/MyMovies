using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMovies.BL.Model;
using System;
using System.Collections.Generic;

namespace MyMovies.BL.Controller.Tests
{
    [TestClass()]
    public class UserControllerTests
    {       
        [TestMethod()]
        public void GetUsersDataTest()
        {
            // Arrange
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var userController = new UserController(userName, password);

            // Act
            List<User> users = userController.GetUsersData();

            // Assert
            Assert.IsNotNull(users);
        }

        [TestMethod()]
        public void SaveTest()
        {
            // Arrange
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            // Act
            var userController = new UserController(userName, password);

            // Assert
            Assert.AreEqual(userName, userController.CurrentUser.Name);
            Assert.AreEqual(password, userController.CurrentUser.Password);
        }

    }
}