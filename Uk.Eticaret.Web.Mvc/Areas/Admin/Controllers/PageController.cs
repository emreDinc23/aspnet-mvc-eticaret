using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class PageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}