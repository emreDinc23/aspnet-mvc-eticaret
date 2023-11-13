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
            var category = _context.Categories.SingleOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var productsInCategory = _context.Products
                .Where(p => p.Categories.Any(cp => cp.CategoryId == category.Id))
                .ToList();
            var categoryName = category.Name;
            ViewBag.CategoryName = categoryName;
            return View(productsInCategory);
        }
    }
}