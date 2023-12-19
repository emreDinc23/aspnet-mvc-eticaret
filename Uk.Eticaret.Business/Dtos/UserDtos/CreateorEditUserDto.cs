using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Dtos.UserDtos
{
    public class CreateorEditUserDto
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
        public List<Persistence.Entities.UserCreditCard> UserCreditCards { get; set; }
    }
}