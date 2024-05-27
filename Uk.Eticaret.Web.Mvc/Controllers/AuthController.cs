using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Uk.Eticaret.Web.Mvc.Models;
using Uk.Eticaret.Persistence.Entities;
using Uk.Eticaret.Web.Mvc.Services.Email;
using Uk.Eticaret.Persistence;

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



        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString())
                        // Add more claims as needed (e.g., roles)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMonths(1)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Generate a random username or use a default logic
                string defaultUsername = "User" + new Random().Next(1000, 9999);

                var user = new User
                {
                    Username = defaultUsername,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Gender = "erkek",
                    Roles = "deneme",
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password // Hash password for security
                    // Additional user properties as needed
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Auth");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                var resetToken = Guid.NewGuid().ToString();

                // Store reset token and expiration in user entity
                user.ResetPasswordToken = resetToken;
                user.ResetPasswordTokenExpiration = DateTime.UtcNow.AddHours(1);

                await _context.SaveChangesAsync();

                var resetLink = Url.Action("ResetPassword", "Auth", new { email = email, token = resetToken }, Request.Scheme);

                await _emailService.SendEmailAsync(email, "Password Reset", $"Reset your password here: {resetLink}");

                return RedirectToAction("PasswordResetSent", "Auth");
            }

            ModelState.AddModelError(string.Empty, "User not found.");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            // Validate email and token, and check token expiration
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.ResetPasswordToken == token);

            if (user == null || user.ResetPasswordTokenExpiration < DateTime.UtcNow)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or token.");
                return View("ForgotPassword");
            }

            var model = new ResetPasswordModel { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.ResetPasswordToken == model.Token);

            if (user == null || user.ResetPasswordTokenExpiration < DateTime.UtcNow)
            {
                return RedirectToAction("Error", "Page", new { message = "An error occurred. Please try again." });
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View(model);
            }

            user.Password = model.Password;
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiration = null;

            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Auth");
        }

        public IActionResult PasswordResetSent()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult TwitterLogin(string returnUrl = "/")
        {
            var redirectUrl = Url.Action("TwitterResponse", "Auth", new { returnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Twitter");
        }

        [HttpGet]
        public async Task<IActionResult> TwitterResponse(string returnUrl = "/")
        {
            var result = await HttpContext.AuthenticateAsync("Twitter");

            if (result.Succeeded)
            {
                var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new Claim(claim.Type, claim.Value)).ToList();

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMonths(1)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Login", "Auth");
        }

    }
}
