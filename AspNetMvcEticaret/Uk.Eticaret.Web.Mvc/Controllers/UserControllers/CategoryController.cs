using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Controllers.UserControllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(int id ,int page)
        {
            return View();
        }
    }
}
