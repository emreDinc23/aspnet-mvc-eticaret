using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.Persistence;

namespace Uk.Eticaret.Web.Mvc.Views.Shared.Components.Heroo
{
    public class HeroViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeroViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productsIsVisibleSlider = _context.Products
                .Include(e => e.Images)
                .Where(pr => pr.IsVisibleSlider)
                .ToList();

            return View(productsIsVisibleSlider);
        }
    }
}