using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Controllers.UserControllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search(string query,int page)
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
            return View();
        }
    }
}
