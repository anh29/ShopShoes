using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        [Route("san-pham")]
        public ActionResult Index(int? id)
        {
            var items = db.Products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            return View(items);
        }
        [Route("danh-muc-san-pham/{alias}-p{id}")]
        public ActionResult ProductCategory(string alias, int id)
        {
            var items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            //ViewBag.CateId là một biến ViewBag được đặt tên là "CateId" -> để lưu trữ dữ liệu tạm thời và được truyền từ controller đến view để hiển thị.
            ViewBag.CateId = id;
            return View(items);
        }
        [Route("chi-tiet/{alias}-{id}")]
        public ActionResult Detail(string alias, int id)
        {
            var item = db.Products.Find(id);
            //if (item != null)
            //{
            //    db.Products.Attach(item);
            //    item.ViewCount = item.ViewCount + 1;
            //    db.Entry(item).Property(x => x.ViewCount).IsModified = true;
            //    db.SaveChanges();
            //}

            return View(item);
        }
        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => (x.IsActive && x.IsHome)).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).ToList();
            return PartialView(items);
        }
    }
}