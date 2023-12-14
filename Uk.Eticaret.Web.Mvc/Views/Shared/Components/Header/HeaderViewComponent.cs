using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.Persistence;
using Uk.Eticaret.Web.Mvc.Models;

namespace Uk.Eticaret.Web.Mvc.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!HttpContext.Session.Keys.Any(e => e == "CartCount"))
                HttpContext.Session.SetInt32("CartCount", 0);

            var categories = _context.Categories
                .Where(b => b.IsActive);

            var categoriesDd = await categories
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .OrderBy(b => b.Text)
                .ToListAsync();

            var vm = new HeaderViewModel()
            {
                CategoriesDd = categoriesDd,
                Categories = categories.ToList(),
            };

            return View(vm);
        }
    }
}