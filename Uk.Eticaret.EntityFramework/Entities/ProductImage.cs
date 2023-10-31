using System.ComponentModel.DataAnnotations;
using Uk.Eticaret.EntityFramework.Entities;

namespace Uk.Eticaret.Web.Mvc.Entity
{
    public class ProductImage : BaseEntity
    {
        [StringLength(500, ErrorMessage = "Lütfen en fazla 500 karakter giriniz.")]
        public string ImageUrl { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}