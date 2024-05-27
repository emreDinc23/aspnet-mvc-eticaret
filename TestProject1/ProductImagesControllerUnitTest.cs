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
    public class ProductImagesControllerUnitTest
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
                var product = new Product
                {
                    ProductName = "Test Product",
                    ProductDescription = "Test Description",
                    ProductColor = "Red",
                    Price = 100,
                    Stock = 10
                };
                var productImage = new ProductImage
                {
                    ImageUrl = "sample-image.jpg",
                    Product = product
                };

                context.Products.Add(product);
                context.ProductImages.Add(productImage);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task Index_ReturnsAViewResult_WithAListOfProductImages()
        {
            // Arrange
            var productId = 1; // Varsayılan product ID
            using (var context = new AppDbContext(_options))
            {
                var controller = new ProductImagesController(context);

                // Act
                var result = await controller.Index(productId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult), "Result is not a ViewResult");

                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult, "ViewResult is null");

                var model = viewResult.Model as List<ProductImage>;
                Assert.IsNotNull(model, "Model is not a List<ProductImage>");

                Assert.AreEqual(1, model.Count, "Expected product image count does not match");
            }
        }

        [TestMethod]
        public async Task Details_ReturnsViewResult_WithCorrectProductImage()
        {
            // Arrange
            var productImageId = 1; // Varsayılan product image ID
            using (var context = new AppDbContext(_options))
            {
                var controller = new ProductImagesController(context);

                // Act
                var result = await controller.Details(productImageId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult), "Result is not a ViewResult");

                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult, "ViewResult is null");

                var model = viewResult.Model as ProductImage;
                Assert.IsNotNull(model, "Model is not a ProductImage");

                Assert.AreEqual(productImageId, model.Id, "Expected product image ID does not match");
            }
        }

        [TestMethod]
        public async Task Create_ReturnsRedirectToActionResult_AfterSuccessfulCreation()
        {
            // Arrange
            var productId = 1; // Varsayılan product ID
            var newProductImage = new ProductImage
            {
                ImageUrl = "new-image.jpg",
                ProductId = productId
            };

            using (var context = new AppDbContext(_options))
            {
                var controller = new ProductImagesController(context);

                // Act
                var result = await controller.Create(productId, newProductImage);

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
            int? productImageId = null; // Null product image ID
            using (var context = new AppDbContext(_options))
            {
                var controller = new ProductImagesController(context);

                // Act
                var result = await controller.Edit(productImageId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult), "Result is not a NotFoundResult");
            }
        }

        [TestMethod]
        public async Task Delete_ReturnsRedirectToActionResult_AfterSuccessfulDeletion()
        {
            // Arrange
            var productImageIdToDelete = 1; // Kaldırılacak product image ID
            using (var context = new AppDbContext(_options))
            {
                var controller = new ProductImagesController(context);

                // Act
                var result = await controller.DeleteConfirmed(productImageIdToDelete);

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
