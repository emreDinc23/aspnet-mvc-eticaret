using Bogus.Extensions.Italy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Business.Services.Abstract;
using Uk.Eticaret.Persistence;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int categoryId)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                // Loglama ekleyin
                Console.WriteLine($"Category not found for ID: {categoryId}");
            }

            return category;
        }

        public async Task<List<Product>> GetProductsInCategoryAsync(int categoryId)
        {
            var productsInCategory = await _context.Products
                .Where(p => p.Categories.Any(cp => cp.CategoryId == categoryId))
                .ToListAsync();

            return productsInCategory;
        }

        public async Task<int> InsertAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);

            if (existingCategory != null)
            {
                // Update category properties
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.Tags = category.Tags;
                existingCategory.Products = category.Products;
                existingCategory.Image = category.Image;
                existingCategory.IsActive = true;

                await _context.SaveChangesAsync();
                return true;
            }

            return false; // Kategori bulunamadı
        }

        public async Task<bool> DeleteByIdAsync(int categoryId)
        {
            var categoryToDelete = await _context.Categories.FindAsync(categoryId);

            if (categoryToDelete != null)
            {
                _context.Categories.Remove(categoryToDelete);
                await _context.SaveChangesAsync();
                return true;
            }

            return false; // Kategori bulunamadı
        }
    }
}