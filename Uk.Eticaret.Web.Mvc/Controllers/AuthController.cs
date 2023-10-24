using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login(string redirectUrl)
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
