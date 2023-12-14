using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Uk.Eticaret.Persistence;
using Uk.Eticaret.Web.Mvc.Models;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart(int id)
        {
            // Session'dan mevcut sepet sayısını al
            var activeCartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;

            // Sepet sayısını bir arttır ve Session'a kaydet
            HttpContext.Session.SetInt32("CartCount", activeCartCount + 1);

            // Veritabanından ürünü çek (id'ye göre)
            var product = _context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                // Session'dan önceki sepeti al, eğer yoksa yeni bir liste oluştur
                var existingCart = HttpContext.Session.GetString("Cart");
                var cartList = string.IsNullOrEmpty(existingCart) ? new List<CartProductModel>() : JsonConvert.DeserializeObject<List<CartProductModel>>(existingCart);

                // CartProductModel oluştur ve ürünü ekle
                var cartProduct = new CartProductModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    UnitPrice = product.Price,
                    ProductImage = product.Images.FirstOrDefault()?.ImageUrl,
                    Quantity = 1
                };

                // Sepet listesine ekle
                cartList.Add(cartProduct);

                // Session'a sepeti JSON formatına dönüştürüp kaydet
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartList));

                // Ürün ID'lerini al, JSON formatına dönüştürüp Session'a kaydet
                var productIds = cartList.Select(p => p.Id).ToList();
                HttpContext.Session.SetString("ProductIds", JsonConvert.SerializeObject(productIds));
            }

            // Ana sayfaya yönlendir
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            try
            {
                // 1. Session'dan sepeti al
                var existingCart = HttpContext.Session.GetString("Cart");
                var cartList = string.IsNullOrEmpty(existingCart) ? new List<CartProductModel>() : JsonConvert.DeserializeObject<List<CartProductModel>>(existingCart);

                // 2. İlgili ürünü sepetten kaldır
                var productToRemove = cartList.FirstOrDefault(p => p.Id == id);
                if (productToRemove != null)
                {
                    cartList.Remove(productToRemove);

                    // 3. Session'a güncellenmiş sepeti kaydet
                    HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartList));
                    var activeCartCount = HttpContext.Session.GetInt32("CartCount") ?? 0;
                    HttpContext.Session.SetInt32("CartCount", activeCartCount - 1);
                    // Başarıyla kaldırıldıysa JSON yanıtı döndür
                    return RedirectToAction("Index", "Cart");
                }
                else
                {
                    // Ürün bulunamadıysa JSON yanıtı döndür
                    return Json(new { success = false, message = "Product not found in the cart." });
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda JSON yanıtı döndür
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}