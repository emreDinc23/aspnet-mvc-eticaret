﻿using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}