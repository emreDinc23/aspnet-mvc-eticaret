using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Uk.Eticaret.Web.Mvc.Areas.Admin.Controllers;
using Uk.Eticaret.Persistence.Entities;
using Uk.Eticaret.Persistence;

namespace TestProject1
{
    [TestClass]
    public class CategoriesControllerUnitTest
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
                var category = new Category
                {
                    
                    Name = "Test Category",
                    Description = "Test Description",
                    Image = "sample-image.jpg", 
                    Tags = "tag1, tag2" 
                };

                context.Categories.Add(category);
                context.SaveChanges();
            }

        }

        [TestMethod]
        public async Task Index_ReturnsAViewResult_WithAListOfCategories()
        {
            // Arrange
            using (var context = new AppDbContext(_options))
            {
                var controller = new CategoriesController(context);

                // Act
                var result = await controller.Index();

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult), "Result is not a ViewResult");

                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult, "ViewResult is null");

                var model = viewResult.Model as List<Category>; // List<Category> olarak alýnmasý
                Assert.IsNotNull(model, "Model is not a List<Category>");

                Assert.AreEqual(1, model.Count, "Expected category count does not match");
            }
        }
        [TestMethod]
        public async Task Details_ReturnsViewResult_WithCorrectCategory()
        {
            // Arrange
            var categoryId = 1; // Test için varsayýlan kategori ID'si
            using (var context = new AppDbContext(_options))
            {
                var controller = new CategoriesController(context);

                // Act
                var result = await controller.Details(categoryId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult), "Result is not a ViewResult");

                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult, "ViewResult is null");

                var model = viewResult.Model as Category;
                Assert.IsNotNull(model, "Model is not a Category");

                Assert.AreEqual(categoryId, model.Id, "Expected category ID does not match");
            }
        }
        [TestMethod]
        public async Task Create_ReturnsRedirectToActionResult_AfterSuccessfulCreation()
        {
            // Arrange
            var newCategory = new Category
            {
                Name = "New Category",
                Description = "New Description",
                Image = "new-image.jpg",
                Tags = "new-tag1, new-tag2",
                IsActive = true
            };
            using (var context = new AppDbContext(_options))
            {
                var controller = new CategoriesController(context);

                // Act
                var result = await controller.Create(newCategory);

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
            int? categoryId = null; // Null category ID
            using (var context = new AppDbContext(_options))
            {
                var controller = new CategoriesController(context);

                // Act
                var result = await controller.Edit(categoryId);

                // Assert
                Assert.IsInstanceOfType(result, typeof(NotFoundResult), "Result is not a NotFoundResult");
            }
        }
        [TestMethod]
        public async Task Delete_ReturnsRedirectToActionResult_AfterSuccessfulDeletion()
        {
            // Arrange
            var categoryIdToDelete = 1; // Kaldýrýlacak kategori ID'si
            using (var context = new AppDbContext(_options))
            {
                var controller = new CategoriesController(context);

                // Act
                var result = await controller.DeleteConfirmed(categoryIdToDelete);

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
