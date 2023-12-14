using Microsoft.AspNetCore.Mvc.Rendering;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Web.Mvc.Models
{
    public class HeaderViewModel
    {
        public List<SelectListItem> CategoriesDd { get; internal set; }
        public List<Category> Categories { get; internal set; }
    }
}