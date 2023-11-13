using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var model = new SearchViewModel
            {
                CategoryId = categoryId,
                SearchTerm = searchTerm,
                CategoriesDd = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList(),
            };

            // Eğer hem kategori hem de ürün adı belirtilmişse
            if (!string.IsNullOrEmpty(categoryId) && int.TryParse(categoryId, out var categoryIdInt) && !string.IsNullOrEmpty(searchTerm))
            {
                // Kategoriye ve ürün adına göre filtreleme yap
                model.Products = _context.Products
                    .Where(p => p.Categories.Any(c => c.CategoryId == categoryIdInt) && p.ProductName.Contains(searchTerm))
                    .ToList();

                // Eğer belirtilen kategori ve ürün adına sahip bir ürün bulunamazsa, tüm ürünleri getir
                if (model.Products.Count == 0)
                {
                    model.Products = _context.Products.ToList();
                    ViewData["NoProductsFound"] = $"Belirtilen kategori ve ürün adına sahip bir ürün bulunamadı. Tüm ürünler listelendi.";
                }
            }
            else if (!string.IsNullOrEmpty(searchTerm))
            {
                // Sadece ürün adına göre filtreleme yap
                model.Products = _context.Products
                    .Where(p => p.ProductName.Contains(searchTerm))
                    .ToList();

                // Eğer belirtilen ürün adına sahip bir ürün bulunamazsa, tüm ürünleri getir
                if (model.Products.Count == 0)
                {
                    model.Products = _context.Products.ToList();
                    ViewData["NoProductsFound"] = $"Belirtilen ürün adına sahip bir ürün bulunamadı. Tüm ürünler listelendi.";
                }
            }
            else
            {
                // Diğer durumlarda, tüm ürünleri getir
                model.Products = _context.Products.ToList();
            }
            ViewBag.ImagesUrl = _context.ProductImages
       .GroupBy(pi => pi.ProductId)
       .ToDictionary(
           group => group.Key,
           group => group.FirstOrDefault()?.ImageUrl
       );

            return View("Search", model);
        }

        [HttpGet("Product/Detail/{slug}")]
        public IActionResult Detail(string slug)
        {
            // ID'ye göre ürünü bul
            var product = _context.Products
                                    .FirstOrDefault(p => p.ProductName == slug);

            if (product == null)
            {
                // Ürün bulunamazsa, örneğin bir hata sayfasına yönlendirme yap
                return RedirectToAction("NotFound", "Error");
            }

            // Modeli doldur
            var viewModel = new ProductDetailViewModel
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductColor = product.ProductColor,
                ProductRating = product.ProductRating,
                Price = product.Price,
                ProductDate = product.ProductDate,
                Stock = product.Stock,

                // Diğer özellikleri ekleyin
            };

            // Ürün detaylarını görünüme geçir
            return View(viewModel);
        }
    }
}