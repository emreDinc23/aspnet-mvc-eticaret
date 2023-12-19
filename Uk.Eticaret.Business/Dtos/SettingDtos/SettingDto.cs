using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Dtos.SettingDtos
{
    public class SettingDto
    {
        public int? UserId { get; set; }
        public User? User { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}