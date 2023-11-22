using Microsoft.AspNetCore.Mvc.Rendering;
using Uk.Eticaret.EntityFramework.Entities;

namespace Uk.Eticaret.Web.Mvc.Models
{
    public class SearchViewModel
    {
        public string SearchTerm { get; internal set; }
        public List<Product> Products { get; internal set; }
    }
}