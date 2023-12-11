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

        [HttpGet("Page/Detail/{slug}")]
        public IActionResult Detail(string slug)
        {
            return View();
        }

        public IActionResult Access(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}