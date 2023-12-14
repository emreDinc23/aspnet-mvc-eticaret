using Microsoft.AspNetCore.Mvc;
using Uk.Eticaret.Business.Services.Abstract;
using Uk.Eticaret.Persistence;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;

        public CategoryController(ICategoryService categoryService, AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> Index(int id, string slug)
        {
            // parametreden gelen değere karşılık gelen category id elde edildi
            var category = await _categoryService.GetByIdAsync(id);
            // parametreyle uyuşan bir değer yoksa hata dönüldü
            if (category == null)
            {
                return NotFound();
            }
            // Category id si parametre değerinden gelen değere uygun olan değerler getirildi
            var productsInCategory = await _categoryService.GetProductsInCategoryAsync(id);

            //Product images tablosundan ImageUrl verisi çekilip viewbag ile taşındı
            ViewBag.ImagesUrl = _context.ProductImages
                .GroupBy(pi => pi.ProductId)
                .ToDictionary(
                group => group.Key,
                group => group.FirstOrDefault()?.ImageUrl
      );

            return View(productsInCategory);
        }
    }
}