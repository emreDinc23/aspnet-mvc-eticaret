using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Uk.Eticaret.Web.Mvc.Areas.Admin.Controllers;
using Uk.Eticaret.Persistence.Entities;
using Uk.Eticaret.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TestProject1
{
    [TestClass]
    public class OrdersControllerUnitTest
    {
        private DbContextOptions<AppDbContext> _options;

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(_options))
            {
                var user = new User
                {
                    FirstName = "Test",
                    LastName = "User",
                    Gender = "Male",
                    Password = "Password123",
                    PhoneNumber = "1234567890",
                    Roles = "User",
                    Username = "testuser",
                    Email = "test@example.com"
                };
                var address = new Address
                {
                    Address1 = "123 Test St",
                    Address2 = "Suite 1",
                    City = "Test City",
                    Country = "Test Country",
                    Phone = "9876543210",
                    PostalCode = "12345",
                    Title = "Home"
                };
                var order = new Order
                {
                    OrderDate = System.DateTime.Now,
                    User = user,
                    ShippingAddress = address
                };

                context.Users.Add(user);
                context.Addresses.Add(address);
                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task Index_ReturnsAViewResult_WithAListOfOrders()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var controller = new OrdersController(context);

                // Act
                var result = await controller.Index();

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult), "Result is not a ViewResult");

                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult, "ViewResult is null");

                var model = viewResult.Model as List<Order>;
                Assert.IsNotNull(model, "Model is not a List<Order>");

                Assert.AreEqual(1, model.Count, "Expected order count does not match");
            }
        }

        [TestMethod]
        public async Task Details_ReturnsViewResult_WithCorrectOrder()
        {
            // Arrange
            var orderId = 1; // Test için varsayılan sipariş ID'si
            using (var context = new AppDbContext(_options))
            {
                var controller = new OrdersController(context);

                // Act
                var result = await controller.Details(orderId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult), "Result is not a ViewResult");

                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult, "ViewResult is null");

                var model = viewResult.Model as Order;
                Assert.IsNotNull(model, "Model is not an Order");

                Assert.AreEqual(orderId, model.Id, "Expected order ID does not match");
            }
        }

        [TestMethod]
        public async Task Create_ReturnsRedirectToActionResult_AfterSuccessfulCreation()
        {
            // Arrange
            var newOrder = new Order
            {
                OrderDate = System.DateTime.Now,
                UserId = 1,
                AddressId = 1
            };

            using (var context = new AppDbContext(_options))
            {
                var controller = new OrdersController(context);

                // Act
                var result = await controller.Create(newOrder);

                // Assert
                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult), "Result is not a RedirectToActionResult");

                var redirectToActionResult = result as RedirectToActionResult;
                Assert.IsNotNull(redirectToActionResult, "RedirectToActionResult is null");

                Assert.AreEqual("Index", redirectToActionResult.ActionName, "Redirect action name is incorrect");
            }
        }

        [TestMethod]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            int? orderId = null; // Null sipariş ID'si
            using (var context = new AppDbContext(_options))
            {
                var controller = new OrdersController(context);

                // Act
                var result = await controller.Edit(orderId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult), "Result is not a NotFoundResult");
            }
        }

        [TestMethod]
        public async Task Delete_ReturnsRedirectToActionResult_AfterSuccessfulDeletion()
        {
            // Arrange
            var orderIdToDelete = 1; // Kaldırılacak sipariş ID'si
            using (var context = new AppDbContext(_options))
            {
                var controller = new OrdersController(context);

                // Act
                var result = await controller.DeleteConfirmed(orderIdToDelete);

                // Assert
                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult), "Result is not a RedirectToActionResult");

                var redirectToActionResult = result as RedirectToActionResult;
                Assert.IsNotNull(redirectToActionResult, "RedirectToActionResult is null");

                Assert.AreEqual("Index", redirectToActionResult.ActionName, "Redirect action name is incorrect");
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
