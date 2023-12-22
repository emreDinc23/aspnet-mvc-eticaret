using System.ComponentModel.DataAnnotations;
using Uk.Eticaret.Persistence.Entities.Abstract;

namespace Uk.Eticaret.Persistence.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            Categories = new List<CategoryProduct>();
        }

        [Required]
        [StringLength(150, ErrorMessage = "Lütfen en fazla 150 karakter giriniz.")]
        public string ProductName { get; set; }

        [MinLength(50, ErrorMessage = "Ürün açıklaması 50 karakterden az olamaz.")]
        [MaxLength(1000, ErrorMessage = "Lütfen en fazla 1000 karakter giriniz.")]
        public string ProductDescription { get; set; }

        public string ProductColor { get; set; }
        public decimal ProductRating { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Tarih formatını doğru giriniz.")]
        public DateTime ProductDate { get; set; }

        public int Stock { get; set; }

        public bool IsActive { get; set; }
        public bool IsVisibleSlider { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<ProductComment>? Comments { get; set; }
        public ICollection<ProductImage>? Images { get; set; }
        public ICollection<CategoryProduct>? Categories { get; set; }
    }
}