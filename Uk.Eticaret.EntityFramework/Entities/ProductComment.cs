using System.ComponentModel.DataAnnotations;
using Uk.Eticaret.EntityFramework.Entities.Abstract;

namespace Uk.Eticaret.EntityFramework.Entities
{
    public class ProductComment : BaseEntity
    {
        [StringLength(500, ErrorMessage = "Lütfen en fazla 500 karakter giriniz.")]
        public string Comment { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}