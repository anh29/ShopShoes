﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        [Route("lien-he")]
        public ActionResult Index()
        {
            return View();
        }
    }
}