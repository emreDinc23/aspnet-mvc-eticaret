using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Uk.Eticaret.EntityFramework;
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
                    ProductName = product.ProductName,
                    UnitPrice = product.Price,
                    ProductImage = product.Images.FirstOrDefault()?.ImageUrl,
                    Quantity = 1
                };

                // Sepet listesine ekle
                cartList.Add(cartProduct);

                // Session'a sepeti tekrar kaydet
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartList));
            }

            // Ana sayfaya yönlendir
            return RedirectToAction("Index", "Cart");
        }
    }
}