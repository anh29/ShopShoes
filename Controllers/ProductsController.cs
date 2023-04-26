using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ShopLearning.Models.ApplicationUser;

namespace ShopLearning.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        
        private ApplicationDbContext db = new ApplicationDbContext();
        [Route("san-pham")]
        public ActionResult Index(int? id)
        {
            var items = db.Product.ToList();
            if(id!= null)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            return View();
        }
        [Route("chi-tiet/{alias}-{id}")]
        public ActionResult Detail(string alias,int? id)
        {
            var items = db.Product.Find(id);
            if(items != null)
            {
                db.Product.Attach(items);
                items.ViewCount = items.ViewCount + 1;
                db.Entry(items).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            return View(items);
        }
        [Route("danh-muc-san-pham/{alias}-p{id}")]
        public ActionResult ProductCategory(string alias, int id)
        {
            var items = db.Product.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();

            }
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            return View(items);

        }
        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Product.Where(x => (x.IsActive && x.IsHome)).Take(14).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductSales()
        {
            var items = db.Product.Where(x => (x.IsActive && x.IsSale)).Take(14).ToList();
            return PartialView(items);
        }
    }
}