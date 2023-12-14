using Microsoft.AspNetCore.Mvc;
using Uk.Eticaret.Business.Services.Abstract;
using Uk.Eticaret.Web.Mvc.Models;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var productList = await _productService.GetAllProductAsync();
            return View(productList);
        }

        public async Task<IActionResult> Search(string categoryId, string searchTerm)
        {
            var filteredProducts = _productService.GetAllQueryable();

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
                model.Products = await _productService.GetAllProductAsync();
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
        public async Task<IActionResult> Detail(string slug)
        {
            var product = await _productService.GetProduct(slug);

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