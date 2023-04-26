using ShopLearning.Models;
using ShopLearning.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ShopLearning.Models.ApplicationUser;

namespace ShopLearning.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_Subcribe()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subcribe(Subcribe req)
        {
            if (ModelState.IsValid)
            {
                db.Subcribes.Add(new Subcribe { Email = req.Email, CreatedDate = DateTime.Now });
                return Json(true);
            }
            return View("Partial_Subcribe");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Refresh()
        {
            var item = new ThongKeModel();
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
            item.HomNay = HttpContext.Application["HomNay"] .ToString();
            item.HomNay = HttpContext.Application["HomQua"] .ToString();
            item.HomNay = HttpContext.Application["TuanNay"] .ToString();
            item.HomNay = HttpContext.Application["TuanTruoc"] .ToString();
            item.HomNay = HttpContext.Application["ThangNay"] .ToString();
            item.HomNay = HttpContext.Application["ThangTruoc"] .ToString();
            item.HomNay = HttpContext.Application["TatCa"] .ToString();
            return PartialView(item);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}