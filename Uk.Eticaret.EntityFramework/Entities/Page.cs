using System.ComponentModel.DataAnnotations;
using Uk.Eticaret.EntityFramework.Entities.Abstract;

namespace Uk.Eticaret.EntityFramework.Entities
{
    public class Page : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}