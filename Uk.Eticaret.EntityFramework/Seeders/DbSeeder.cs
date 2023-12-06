using Bogus;
using Uk.Eticaret.EntityFramework.Entities;

namespace Uk.Eticaret.EntityFramework.Seeders
{
    public class DbSeeder
    {
        public static void SeedAll(AppDbContext context)
        {
            //SeedProducts(context);
            SeedCategoriesWithProducts(context);
            SeedUsers(context);
        }

        //public static void SeedProducts(AppDbContext context)
        //{
        //    if (!context.Products.Any())
        //    {
        //        var productsTr = new Bogus.DataSets.Commerce(locale: "tr");
        //        var productFaker = new Faker<Product>()
        //            .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
        //            .RuleFor(p => p.ProductDescription, f => f.Lorem.Sentence())
        //            .RuleFor(p => p.ProductColor, f => f.Commerce.Color())
        //            .RuleFor(p => p.ProductRating, f => f.Random.Decimal(1, 5))
        //            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
        //            .RuleFor(p => p.ProductDate, f => f.Date.Recent())
        //            .RuleFor(p => p.Stock, f => f.Random.Int(0, 100))
        //            .RuleFor(p => p.IsActive, f => f.Random.Bool())
        //            .RuleFor(p => p.IsVisibleSlider, f => f.Random.Bool());

        //        var products = productFaker.Generate(20);

        //        context.Products.AddRange(products);
        //        context.SaveChanges();

        //        if (!context.ProductImages.Any())
        //        {
        //            var imageFaker = new Faker<ProductImage>()
        //                .RuleFor(pi => pi.ImageUrl, f => f.Image.PicsumUrl())
        //                .RuleFor(pi => pi.ProductId, f => f.PickRandom(products).Id);

        //            var images = imageFaker.Generate(40);

        //            context.ProductImages.AddRange(images);
        //            context.SaveChanges();
        //        }
        //    }
        //}

        public static void SeedCategoriesWithProducts(AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categoryFaker = new Faker<Category>()
                    .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                    .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                    .RuleFor(c => c.Image, f => f.Image.PicsumUrl())
                    .RuleFor(c => c.Tags, f => f.Commerce.Categories(1)[0])
                    .RuleFor(c => c.IsActive, f => f.Random.Bool());

                var categories = categoryFaker.Generate(10);

                foreach (var category in categories)
                {
                    context.Categories.Add(category);
                    context.SaveChanges();

                    var productFaker = new Faker<Product>()
                        .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
                        .RuleFor(p => p.ProductDescription, f => f.Lorem.Sentence())
                        .RuleFor(p => p.ProductColor, f => f.Commerce.Color())
                        .RuleFor(p => p.ProductRating, f => f.Random.Decimal(1, 5))
                        .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
                        .RuleFor(p => p.ProductDate, f => f.Date.Recent())
                        .RuleFor(p => p.Stock, f => f.Random.Int(0, 100))
                        .RuleFor(p => p.IsActive, f => f.Random.Bool())
                        .RuleFor(p => p.IsVisibleSlider, f => f.Random.Bool());

                    var products = productFaker.Generate(5);

                    foreach (var product in products)
                    {
                        context.Products.Add(product);
                        context.SaveChanges();

                        var imageFaker = new Faker<ProductImage>()
                            .RuleFor(pi => pi.ImageUrl, f => f.Image.PicsumUrl())
                            .RuleFor(pi => pi.ProductId, f => product.Id);

                        var images = imageFaker.Generate(1);

                        context.ProductImages.AddRange(images);
                        context.SaveChanges();

                        context.CategoryProducts.Add(new CategoryProduct { CategoryId = category.Id, ProductId = product.Id });
                        context.SaveChanges();
                    }
                }
            }
        }

        public static void SeedUsers(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
            {
                new User {
                    FirstName = "John",
                    LastName = "Doe",
                    Username = "johndoe",
                    Email = "john@example.com",
                    Password = "password123",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Gender = "Erkek",
                    IsActive = true },

                new User {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Username = "janesmith",
                    Email = "jane@example.com",
                    Password = "securepassword",
                    DateOfBirth = new DateTime(1992, 3, 15),
                    Gender = "Kadın",
                    IsActive = true },
                 };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}