using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(string slug, int page)
        {
            return View();
        }
    }
}
