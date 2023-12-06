using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class PageController : Controller
    {
        [Route("/contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Detail(string slug)
        {
            return View();
        }
    }
}