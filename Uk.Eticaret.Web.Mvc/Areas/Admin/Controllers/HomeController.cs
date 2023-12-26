using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.Persistence;
using Uk.Eticaret.Web.Mvc.Areas.Admin.Model;

namespace Uk.Eticaret.Web.Mvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new StatisticsViewModel()
            {
                TotalProductCount = _context.Products.CountAsync().Result,
                TotalCategoryCount = _context.Categories.CountAsync().Result,
                TotalOrderCount = _context.Orders.CountAsync().Result,
                TotalUserCount = _context.Users.CountAsync().Result
            };

            return View(viewModel);
        }
    }
}