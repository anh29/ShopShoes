using ShopLearning.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ShopLearning.Models.ApplicationUser;

namespace ShopLearning.Areas.Admin.Controllers
{
    public class ProductImagesController : Controller
    {
        // GET: Admin/ProductImages
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int productid)
        {
            ViewBag.ProductId = productid;
            var item = db.ProductImages.Where(x => x.ProductId == productid).ToList();
            return View(item);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductImages.Find(id);
            db.ProductImages.Remove(item);
            db.SaveChanges();
            return Json(new {success=true});
        }
        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            db.ProductImages.Add(new ProductImage
            {
                ProductId = productId,
                Image = url,
                IsDefault = false,

            }) ;
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}