using System.ComponentModel.DataAnnotations;

namespace Uk.Eticaret.Web.Mvc.Models
{
    public class Category
    {        
        public int Id { get; set; }
        [Required]
        [StringLength(200)]

        public string CategoryName { get; set; }
        [StringLength (500)]
        public string CategoryDescription { get; set; }        
        public string CategoryImage { get; set; }

        public string CategoryTags { get; set; }
        public bool CategoryStatus { get; set; }

    }
}
