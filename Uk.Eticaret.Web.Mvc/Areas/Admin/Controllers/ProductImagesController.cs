using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Uk.Eticaret.Persistence;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductImagesController : Controller
    {
        private readonly AppDbContext _context;

        public ProductImagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductImages
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.ProductId = id;
            var appDbContext = _context.ProductImages.Where(p => p.ProductId == id);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/ProductImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductImages == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        public IActionResult Create(int id)
        {
            // ProductId'yi ViewBag üzerinden taşı
            ViewBag.ProductId = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ImageUrl")] ProductImage productImage)
        {
            if (!ModelState.IsValid)
            {
                // İlgili Product'ı bul
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    // İlgili Product bulunamazsa hata mesajı döndür
                    ModelState.AddModelError("ProductId", "Belirtilen ürün bulunamadı.");
                    return View(productImage);
                }
                if (product.Images == null)
                {
                    product.Images = new List<ProductImage>();
                }
                product.Images.Add(productImage);

                // ProductImage'yi ekleyip kaydet
                product.Images.Add(productImage);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { id = productImage.ProductId });
            }

            // ModelState geçerli değilse, Create view'ına geri dön
            return View(productImage);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductImages == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageUrl,Id")] ProductImage updatedProductImage)
        {
            if (id != updatedProductImage.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var existingProductImage = await _context.ProductImages.FindAsync(id);
                try
                {
                    // Mevcut ProductImage'i veritabanından çek

                    if (existingProductImage == null)
                    {
                        return NotFound();
                    }

                    // Sadece ImageUrl alanını güncelle
                    existingProductImage.ImageUrl = updatedProductImage.ImageUrl;

                    // Değişiklikleri kaydet
                    _context.Update(existingProductImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductImageExists(updatedProductImage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = existingProductImage.ProductId });
            }
            return View(updatedProductImage);
        }

        // GET: Admin/ProductImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductImages == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // POST: Admin/ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductImages == null)
            {
                return Problem("Entity set 'AppDbContext.ProductImages'  is null.");
            }
            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage != null)
            {
                _context.ProductImages.Remove(productImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = productImage.ProductId });
        }

        private bool ProductImageExists(int id)
        {
            return (_context.ProductImages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}