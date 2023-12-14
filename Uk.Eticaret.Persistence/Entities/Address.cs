using System.ComponentModel.DataAnnotations;
using Uk.Eticaret.Persistence.Entities.Abstract;

namespace Uk.Eticaret.Persistence.Entities
{
    public class Address : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Lütfen en fazla 100 karakter giriniz.")]
        public string City { get; set; }

        [StringLength(100, ErrorMessage = "Lütfen en fazla 100 karakter giriniz.")]
        [Required]
        public string Address1 { get; set; }

        [StringLength(100, ErrorMessage = "Lütfen en fazla 100 karakter giriniz.")]
        [Required]
        public string Address2 { get; set; }

        [StringLength(30, ErrorMessage = "Lütfen en fazla 30 karakter giriniz.")]
        [Required]
        public string PostalCode { get; set; }

        [StringLength(200, ErrorMessage = "Lütfen en fazla 200 karakter giriniz.")]
        [Required]
        public string Country { get; set; }

        [StringLength(50, ErrorMessage = "Lütfen en fazla 50 karakter giriniz.")]
        [Required]
        public string Phone { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}