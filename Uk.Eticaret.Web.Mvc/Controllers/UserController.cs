﻿using Microsoft.AspNetCore.Mvc;

namespace Uk.Eticaret.Web.Mvc.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Adresses()
        {
            return View();
        }
        public IActionResult Orders()
        {
            return View();
        }
    }
}
