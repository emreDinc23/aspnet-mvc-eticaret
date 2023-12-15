using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business.Dtos.CategoryDtos
{
    public class ViewCategoryDto
    {
        public ViewCategoryDto()
        {
            Products = new List<CategoryProduct>();
        }

        [Required]
        [StringLength(200, ErrorMessage = "Lütfen en fazla 200 karakter giriniz.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Lütfen en fazla 500 karakter giriniz.")]
        public string Description { get; set; }

        [StringLength(1000, ErrorMessage = "Lütfen en fazla 1000 karakter giriniz.")]
        public string Image { get; set; }

        [StringLength(50, ErrorMessage = "Lütfen en fazla 50 karakter giriniz.")]
        public string Tags { get; set; }

        public bool IsActive { get; set; }

        public ICollection<CategoryProduct> Products { get; set; }
    }
}