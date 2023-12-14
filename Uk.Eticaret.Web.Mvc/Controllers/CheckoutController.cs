using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Uk.Eticaret.Persistence;
using Uk.Eticaret.Persistence.Entities;
using Uk.Eticaret.Web.Mvc.Models;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(IFormCollection form)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userIdInt = int.Parse(userId);
            var user = _context.Users
                .Include(u => u.Addresses)
                .Include(u => u.Orders)
                .FirstOrDefault(u => u.Id == userIdInt);

            // Eğer kullanıcı bulunduysa
            if (user != null)
            {
                // Formdan gelen adres bilgilerini kullanarak yeni bir adres oluştur
                var newAddress = new Address
                {
                    Title = form["Title"],
                    Phone = form["Phone"],
                    Address1 = form["Address1"],
                    Address2 = form["Address2"],
                    City = form["City"],
                    PostalCode = form["PostalCode"],
                    Country = form["Country"],
                };

                // Adresi kullanıcıya ekle
                user.Addresses.Add(newAddress);
                if (user.UserCreditCards == null)
                {
                    user.UserCreditCards = new List<UserCreditCard>(); // Eğer null ise, yeni bir liste oluştur
                }
                var newUserCreditCart = new UserCreditCard
                {
                    CardHolderName = form["cardHolder"],
                    CardNumber = form["CardNumber"],
                    ExpirationMonth = form["ExpirationMonth"],
                    ExpirationYear = form["ExpirationYear"],
                    cvc = form["cvc"],
                };
                user.UserCreditCards.Add(newUserCreditCart);

                // Sepetten sipariş bilgilerini al
                var existingCart = HttpContext.Session.GetString("Cart");
                List<OrderProduct> cartProducts;

                if (!string.IsNullOrEmpty(existingCart))
                {
                    cartProducts = JsonConvert.DeserializeObject<List<OrderProduct>>(existingCart);
                }
                else
                {
                    // Sepet boşsa, hata mesajı
                    return BadRequest("Sepet boş.");
                }

                // Yeni bir sipariş oluştur
                var newOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    ShippingAddress = newAddress,
                    OrderProducts = new List<OrderProduct>()  // Boş bir liste oluştur
                };
                var productIdsJson = HttpContext.Session.GetString("ProductIds");
                var productIds = JsonConvert.DeserializeObject<List<int>>(productIdsJson);
                foreach (var cartProductId in productIds)
                {
                    // Ürünü veritabanından al, eğer bulunamazsa hata mesajı döndür
                    var productFromDb = _context.Products.Find(cartProductId);

                    if (productFromDb == null)
                    {
                        return BadRequest("Ürün bulunamadı.");
                    }

                    // Sepetteki ürünün miktarını al
                    var cartProduct = JsonConvert.DeserializeObject<List<CartProductModel>>(existingCart).FirstOrDefault(p => p.Id == cartProductId);
                    var quantity = cartProduct?.Quantity ?? 1;

                    // Her bir OrderProduct için yeni bir OrderProduct oluştur ve Order'a ekle
                    newOrder.OrderProducts.Add(new OrderProduct
                    {
                        Product = productFromDb,
                        Quantity = quantity,
                        Price = productFromDb.Price
                    });
                }
                // Siparişi kullanıcıya ekle
                user.Orders.Add(newOrder);

                // Veritabanına değişiklikleri kaydet
                await _context.SaveChangesAsync();

                // Sepeti temizle
                HttpContext.Session.Remove("Cart");
                var activeCartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;
                HttpContext.Session.SetInt32("CartCount", activeCartCount - 1);

                return RedirectToAction("Success");
            }
            else
            {
                // Kullanıcı bulunamazsa, hata mesajı döndür
                return BadRequest("Kullanıcı bulunamadı.");
            }
        }
    }
}