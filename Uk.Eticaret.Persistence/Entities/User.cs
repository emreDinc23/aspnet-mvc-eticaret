using System.ComponentModel.DataAnnotations;
using Uk.Eticaret.Persistence.Entities.Abstract;

namespace Uk.Eticaret.Persistence.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(30, ErrorMessage = "Lütfen en fazla 30 karakter giriniz.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Lütfen en fazla 30 karakter giriniz.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Lütfen en fazla 30 karakter giriniz.")]
        public string Username { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Lütfen en fazla 30 karakter giriniz.")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiration { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }
        public string Roles { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public List<UserCreditCard> UserCreditCards { get; set; }
    }
}