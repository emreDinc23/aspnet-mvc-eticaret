using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Dtos.UserCreditCard
{
    public class CreateorEditUserCreditCardDto
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string cvc { get; set; }
        public ICollection<User> Users { get; set; }
    }
}