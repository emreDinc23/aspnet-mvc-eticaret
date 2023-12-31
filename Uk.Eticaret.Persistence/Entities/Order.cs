﻿using System.ComponentModel.DataAnnotations;
using Uk.Eticaret.Persistence.Entities.Abstract;

namespace Uk.Eticaret.Persistence.Entities
{
    public class Order : BaseEntity
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