using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uk.Eticaret.Web.Mvc.Models;
using Uk.Eticaret.Persistence;
using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.Persistence.Entities;
using Uk.Eticaret.Web.Mvc.Services.Email;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailServices _emailService;

        public AuthController(AppDbContext context, IEmailServices emailService)
        {
            _context = context;
            _emailService = emailService;
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
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            string defaultUsername = "User";
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            if (ModelState.IsValid)
            {
                // Kullanıcıyı kaydetme işlemi
                var user = new User
                {
                    Username = defaultUsername + randomNumber.ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password, // Şifreyi güvenli bir şekilde saklamak için gerekirse uygun yöntem kullanılmalıdır
                    Gender = "",
                    Roles = ""
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Kayıt işlemi başarılı, giriş yap sayfasına yönlendir
                return RedirectToAction("Login", "Auth");
            }

            // ModelState geçerli değilse, hata mesajları ile birlikte aynı sayfaya geri dön
            return View(model);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            // Kullanıcıyı e-posta adresine göre bul
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

            if (user != null)
            {
                try
                {
                    // Kullanıcıyı bulduk, şimdi şifre sıfırlama token'ı oluşturmalıyız
                    var resetToken = Guid.NewGuid().ToString();

                    // Token'ı kullanıcıya bağlı olarak saklamalısınız (örneğin, veritabanında bir alan olarak)
                    user.ResetPasswordToken = resetToken;
                    await _context.SaveChangesAsync();

                    // Şifre sıfırlama bağlantısını içeren bir e-posta gönder
                    var resetLink = Url.Action("ResetPassword", "Auth", new { email = email, token = resetToken }, Request.Scheme);

                    // E-posta gönderme işlemi (IEmailService kullanılarak)
                    await _emailService.SendEmailAsync(email, "Şifre Sıfırlama", $"Şifre sıfırlama bağlantısı: {resetLink}");

                    user.ResetPasswordTokenExpiration = DateTime.UtcNow.AddHours(1);
                    // E-posta gönderildiğini varsayalım
                    return RedirectToAction("Access", "Page", new { message = "Şifre Sıfırlama bağlantısı Mailinize gönderildi (E posta adresinizi kontrol ediniz" });
                }
                catch (Exception)
                {
                    // Hata durumunda uygun bir işlem yapabilirsiniz
                    ModelState.AddModelError(string.Empty, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                    return View("ForgotPassword");
                }
            }

            // Kullanıcı bulunamadı
            ModelState.AddModelError(string.Empty, "Bu e-posta adresine kayıtlı bir kullanıcı bulunamadı.");
            return View("ForgotPassword");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            // Eğer email veya token boşsa veya kullanıcı bulunamazsa, hata sayfasına yönlendir
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Error", "Page", new { message = "Hatalı veri girdiniz bilgileri kontrol ediniz..." });
            }

            // Kullanıcıyı email ve token'a göre bul
            var user = _context.Users.FirstOrDefault(e => e.Email == email && e.ResetPasswordToken == token);

            // Kullanıcı bulunamazsa veya token süresi geçerli değilse, hata sayfasına yönlendir
            if (user == null || user.ResetPasswordTokenExpiration < DateTime.UtcNow)
            {
                ModelState.AddModelError(string.Empty, "Url geçerli değil tekrar deneyiniz");
                return View("ForgotPassword");
            }

            // Şifre sıfırlama sayfasını göster
            return View(new ResetPasswordModel { Email = email, Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            // Kullanıcıyı, email ve token'a göre veritabanında bul
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == model.Email && e.ResetPasswordToken == model.Token);

            // Kullanıcı bulunamazsa veya token süresi geçerli değilse, hata sayfasına yönlendir
            if (user == null || user.ResetPasswordTokenExpiration < DateTime.UtcNow)
            {
                return RedirectToAction("Error", "Page", new { message = "Bir hata oluştu tekrar deneyiniz.." });
            }

            // Yeni şifre ve şifre onayı kontrolü
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Şifreler eşleşmiyor.");
                return View(model);
            }

            // Yeni şifreyi kullanıcının şifresi olarak güncelle
            user.Password = model.Password;

            // Şifre sıfırlama token'ını ve süresini temizle
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiration = null;

            // Veritabanında güncelleme işlemi
            await _context.SaveChangesAsync();

            // Şifre sıfırlama işlemi başarılıysa, giriş sayfasına yönlendir
            return RedirectToAction("Access", "Page", new { message = "Şifreniz başarıyla sıfırlandı. Login sayfasına yönlendiriliyorsunuz..." });
        }

        public IActionResult PasswordResetSent()
        {
            // Şifre sıfırlama bağlantısı gönderildi sayfasını göster
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