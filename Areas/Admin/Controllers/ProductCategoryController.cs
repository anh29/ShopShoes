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
    public class ProductCategoryController : Controller
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();
        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var items = _dal.GetAll<ProductCategory>();
            return View(items);
        }


        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = ShopOnline.Models.Common.Filter.FilterChar(model.Title).Replace(".", "%");
                _dal.Add(model);
                _dal.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            var item = _dal.GetById<ProductCategory>(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                _dal.Update(model);
                _dal.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dal.GetById<ProductCategory>(id);
            if (item != null)
            {
                _dal.Delete(item);
                _dal.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    var entities = items.Select(item => _dal.GetById<ProductCategory>(Convert.ToInt32(item))).ToList();
                    _dal.DeleteAll(entities);
                    _dal.SaveChanges();
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}
