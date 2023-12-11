﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.EntityFramework;
using Uk.Eticaret.Web.Mvc.Models;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string categoryId, string searchTerm)
        {
            var filteredProducts = _context.Products.Include(e => e.Images).Include(c => c.Comments).Select(e => e);

            // Kategoriye göre filtreleme
            if (!string.IsNullOrEmpty(categoryId) && int.TryParse(categoryId, out var categoryIdInt))
            {
                filteredProducts = filteredProducts.Where(p => p.Categories.Any(c => c.CategoryId == categoryIdInt));
            }

            // Ürün adına göre filtreleme
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredProducts = filteredProducts.Where(p => p.ProductName.Contains(searchTerm));
            }

            // Sonuçları listele
            var model = new ProductListViewModel();
            model.SearchTerm = searchTerm;
            model.Products = filteredProducts.ToList();

            // Eğer belirtilen kriterlere uygun ürün bulunamazsa, tüm ürünleri getir
            if (model.Products.Count == 0)
            {
                model.Products = _context.Products.Include(e => e.Comments).Include(c => c.Images).ToList();
                string errorMessage = string.IsNullOrEmpty(categoryId)
                    ? $"Belirtilen isimdeki ürünler bulunamadı. Tüm ürünler listelendi."
                    : $"Belirtilen kategori ve isimdeki ürünler bulunamadı. Tüm ürünler listelendi.";

                ViewData["NoProductsFound"] = errorMessage;
            }

            //ViewBag.ImagesUrl = _context.ProductImages
            //    .GroupBy(pi => pi.ProductId)
            //    .ToDictionary(
            //        group => group.Key,
            //        group => group.FirstOrDefault()?.ImageUrl
            //    );

            return View("Search", model);
        }

        [HttpGet("Product/Detail/{slug}")]
        public IActionResult Detail(string slug)
        {
            // ID'ye göre ürünü bul
            var product = _context.Products
        .Include(e => e.Images)
        .Include(e => e.Categories)
            .ThenInclude(cp => cp.Category) // Kategori bilgisini ekleyin
        .FirstOrDefault(p => p.ProductName == slug);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailViewModel
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductColor = product.ProductColor,
                ProductRating = product.ProductRating,
                Price = product.Price,
                ProductDate = product.ProductDate,
                Stock = product.Stock,
                ImagesUrl = product.Images.Select(img => img.ImageUrl).ToList(),
                CategoryName = product.Categories.FirstOrDefault()?.Category?.Name,
                CategoryId = (int)(product.Categories.FirstOrDefault()?.Category?.Id)
            };

            return View(viewModel);
        }
    }
}