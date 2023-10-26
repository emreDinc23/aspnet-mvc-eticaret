using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
