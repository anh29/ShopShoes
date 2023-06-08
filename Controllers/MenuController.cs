using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Data;
using ShopOnline.Models.EF;

namespace ShopOnline.Controllers
{
    public class MenuController : Controller
    {
        private readonly DataAccessLayer _dal;

        public MenuController()
        {
            _dal = new DataAccessLayer();
        }

        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuTop()
        {
            var item = _dal.GetAll<Category>().OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop", item);
        }

        public ActionResult MenuProductCategory()
        {
            var item = _dal.GetAll<ProductCategory>().ToList();
            return PartialView("_MenuProductCategory", item);
        }

        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var item = _dal.GetAll<ProductCategory>().ToList();
            return PartialView("_MenuLeft", item);
        }

        public ActionResult MenuArrivals()
        {
            var item = _dal.GetAll<ProductCategory>().ToList();
            return PartialView("_MenuArrivals", item);
        }
    }
}
