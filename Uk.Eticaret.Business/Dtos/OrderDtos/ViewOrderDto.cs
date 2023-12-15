using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Dtos.OrderDtos
{
    public class CreateorEditOrderDto
    {
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Tarih formatını doğru giriniz.")]
        public DateTime OrderDate { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public int AddressId { get; set; }
        public Address ShippingAddress { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}