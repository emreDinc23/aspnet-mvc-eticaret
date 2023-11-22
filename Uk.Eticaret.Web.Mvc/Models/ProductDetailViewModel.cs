﻿using Uk.Eticaret.EntityFramework.Entities;

namespace Uk.Eticaret.Web.Mvc.Models
{
    public class ProductDetailViewModel
    {
        public string ProductName { get; internal set; }

        public string ProductDescription { get; internal set; }

        public string ProductColor { get; internal set; }
        public string ImageUrl { get; internal set; }
        public decimal ProductRating { get; internal set; }

        public decimal Price { get; internal set; }

        public DateTime ProductDate { get; internal set; }

        public int Stock { get; internal set; }
        public string categoryName { get; internal set; }
    }
}