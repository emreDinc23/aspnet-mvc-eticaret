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
                        IsActive = true },
                    new Category {
                        Name = "Beyaz Eşya",
                        Description = "Bu bir başka örnek kategori açıklamasıdır.",
                        Image = "kategori2.jpg",
                        Tags = "etiket4, etiket5",
                        IsActive = true }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }

        public static void SeedProducts(AppDbContext context)
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>
            {
                new Product {   ProductName = "Buzdolabı",
                    ProductDescription = "Bu bir örnek ürün açıklamasıdır.",
                    ProductColor = "Beyaz",
                    ProductRating = "5 yıldız",
                    Price = 100,
                    ProductDate = new DateTime(2023, 10, 30),
                    Stock = 50,
                    IsActive = true },
                new Product {  ProductName = "Bilgisayar",
                    ProductDescription = "Bu bir başka örnek ürün açıklamasıdır.",
                    ProductColor = "Siyah",
                    ProductRating = "4 yıldız",
                    Price = 75,
                    ProductDate = new DateTime(2023, 10, 30),
                    Stock = 30,
                    IsActive = true },
                    };

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