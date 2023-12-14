using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uk.Eticaret.Business.Services.Abstract;
using Uk.Eticaret.Persistence;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            var products = await _context.Products
                .Include(e => e.Images)
                .Include(c => c.Comments)
                .ToListAsync();

            return products;
        }

        public async Task<Product> GetProduct(string slug)
        {
            var product = _context.Products
            .Include(e => e.Images)
            .Include(e => e.Categories)
            .ThenInclude(cp => cp.Category)
            .FirstOrDefault(p => p.ProductName == slug);

            return product;
        }

        public void CreateProductAsync(Product createProduct)
        {
            _context.Products.Add(createProduct);
            _context.SaveChanges();
        }

        public void UpdateProductAsync(Product updateProduct)
        {
            _context.Products.Update(updateProduct);
            _context.SaveChanges();
        }

        public void DeleteProductAsync(int id)
        {
            var productToDelete = _context.Products.Find(id);

            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                _context.SaveChanges();
            }
        }

        public IQueryable<Product> GetAllQueryable()
        {
            var filteredProducts = _context.Products.Include(e => e.Images).Include(c => c.Comments).Select(e => e);
            return filteredProducts;
        }
    }
}