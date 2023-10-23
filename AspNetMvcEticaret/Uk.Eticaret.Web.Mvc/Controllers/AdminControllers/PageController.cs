using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Controllers.AdminControllers
{
    public class PageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
