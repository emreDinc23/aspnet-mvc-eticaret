using Bogus;
using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.EntityFramework.Entities;

namespace Uk.Eticaret.EntityFramework.Seeders
{
    public class DbSeeder
    {
        public static void SeedAll(AppDbContext context)
        {
            SeedCategories(context);
            SeedProducts(context);
            SeedUsers(context);
        }

        public static void SeedCategories(AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category {
                        Name = "Elektronik",
                        Description = "Bu bir örnek kategori açıklamasıdır.",
                        Image = "kategori1.jpg",
                        Tags = "etiket1, etiket2, etiket3",
                        Status = true },
                    new Category {
                        Name = "Beyaz Eşya",
                        Description = "Bu bir başka örnek kategori açıklamasıdır.",
                        Image = "kategori2.jpg",
                        Tags = "etiket4, etiket5",
                        Status = true }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }

        public static void SeedProducts(AppDbContext context)
        {
            if (!context.Products.Any())
            {
                int i = 1;
                var productsTr = new Bogus.DataSets.Commerce(locale: "tr");
                var productFaker = new Faker<Product>()
                    .RuleFor(o => o.ProductName, f => productsTr.ProductName())
                    .RuleFor(o => o.ProductDescription, f => productsTr.ProductDescription())
                    .RuleFor(o => o.ProductRating, f => f.Random.Decimal(1, 5))
                    .RuleFor(o => o.ProductColor, f => f.Commerce.Color())
                    .RuleFor(o => o.Price, f => f.Random.Decimal(10, 100))
                    .RuleFor(o => o.Stock, f => f.Random.Int(10, 100))
                    .RuleFor(o => o.ProductDate, new DateTime(2023, 05, 14, 9, 41, 25))
                    .RuleFor(o => o.IsActive, true)
                    .RuleFor(o => o.IsVisibleSlider, f => f.Random.Bool());

                var products = productFaker.Generate(5);
                context.Products.AddRange(products);
                context.SaveChanges();
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