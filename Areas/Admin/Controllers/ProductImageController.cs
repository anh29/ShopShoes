using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Data;
using ShopOnline.Models;
using ShopOnline.Models.EF;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductImageController : Controller
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();
        // GET: Admin/ProductImage
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = _dal.GetFiltered<ProductImage>(x => x.ProductId == id);
            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            _dal.Add(new ProductImage
            {
                ProductId = productId,
                Image = url,
                IsDefault = false
            });
            _dal.SaveChanges();
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dal.GetById<ProductImage>(id);
            _dal.Delete(item);
            _dal.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult SetDefault(int id, bool isDefault)
        {
            var image = _dal.GetById<ProductImage>(id);

            if (image != null)
            {
                image.IsDefault = isDefault;
                _dal.Update(image);
                _dal.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}