using Microsoft.AspNetCore.Mvc.Rendering;
using Uk.Eticaret.EntityFramework.Entities;

namespace Uk.Eticaret.Web.Mvc.Models
{
    public class SearchViewModel
    {
        public string CategoryId { get; set; }
        public string SearchTerm { get; set; }
        public List<SelectListItem> CategoriesDd { get; set; }
        public List<Product> Products { get; set; }
    }
}