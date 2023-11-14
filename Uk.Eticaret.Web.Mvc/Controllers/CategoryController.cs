using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.EntityFramework;
using Uk.Eticaret.EntityFramework.Entities;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id, string slug)
        {
            // parametreden gelen değere karşılık gelen category id elde edildi
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);
            // parametreyle uyuşan bir değer yoksa hata dönüldü
            if (category == null)
            {
                return NotFound();
            }
            // Category id si parametre değerinden gelen değere uygun olan değerler getirildi
            var productsInCategory = _context.Products
                .Where(p => p.Categories.Any(cp => cp.CategoryId == category.Id))
                .ToList();

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