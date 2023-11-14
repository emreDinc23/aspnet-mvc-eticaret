using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.EntityFramework;

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
            var productsIsVisibleSlider = _context.Products.Where(pr => pr.IsVisibleSlider).ToList();
            ViewBag.ImagesUrl = _context.ProductImages
      .GroupBy(pi => pi.ProductId)
      .ToDictionary(
          group => group.Key,
          group => group.FirstOrDefault()?.ImageUrl
      );
            return View(productsIsVisibleSlider);
        }
    }
}