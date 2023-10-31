using System.ComponentModel.DataAnnotations;
using Uk.Eticaret.EntityFramework.Entities;

namespace Uk.Eticaret.Web.Mvc.Entity
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Products = new List<CategoryProduct>();
        }

        [Required]
        [StringLength(200, ErrorMessage = "Lütfen en fazla 200 karakter giriniz.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Lütfen en fazla 500 karakter giriniz.")]
        public string Description { get; set; }

        [StringLength(1000, ErrorMessage = "Lütfen en fazla 1000 karakter giriniz.")]
        public string Image { get; set; }

        [StringLength(50, ErrorMessage = "Lütfen en fazla 50 karakter giriniz.")]
        public string Tags { get; set; }

        public bool Status { get; set; }

        public ICollection<CategoryProduct> Products { get; set; }
    }
}