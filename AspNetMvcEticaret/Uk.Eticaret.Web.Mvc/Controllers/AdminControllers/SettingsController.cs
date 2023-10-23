using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Controllers.AdminControllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
