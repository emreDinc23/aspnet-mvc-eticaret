namespace Uk.Eticaret.Web.Mvc.Models
{
    public static class Helper
    {
        public static string GetProductImage(string path)
        {
            if (path.Contains("http"))
                return path;
            else
                return "/template/assets/images/product/" + path;
        }
    }
}