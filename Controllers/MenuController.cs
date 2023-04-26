using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ShopLearning.Models.ApplicationUser;

namespace ShopLearning.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuTop()
        {
            var items = db.Categories.OrderBy(x => x.Position).ToList();
            return PartialView("MenuTop", items);
        }
        public ActionResult MenuTopCategory()
        {
            var items = db.ProductCategories.ToList();
            return PartialView("MenuTopCategory", items);
        }
        public ActionResult MenuArrivals()
        {
            var items = db.ProductCategories.ToList();
            return PartialView("MenuArrivals", items);
        }
        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var items = db.ProductCategories.ToList();
            return PartialView("MenuLeft", items);
        }
    }
}