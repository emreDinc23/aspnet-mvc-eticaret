using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uk.Eticaret.Web.Mvc.Models;
using Uk.Eticaret.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "/")
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == model.Email && e.Password == model.Password);
            // Kullanıcıyı doğrula ve cookie oluştur
            if (user != null)
            {
                // Kimlik Bilgisi
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString()),
                };

                if (user.Roles != null)
                {
                    var roles = user.Roles.Split(',');
                    foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Kimlik
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Cüzdan
                var identityPrinciple = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMonths(1)
                };

                // Giriş yap
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    identityPrinciple,
                    authProperties);

                return LocalRedirect(returnUrl);
            }
            else
            {
                // Kullanıcı adı veya şifre hatalı, hata mesajını göster
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            // Cookie'yi sil ve kullanıcıyı çıkış yap
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            // Erişim reddedildi sayfasını göster
            return View();
        }
    }
}