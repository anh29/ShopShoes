using ShopOnline.Data;
using ShopOnline.Models;
using ShopOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();
        // GET: Admin/Category
        //Phương thức Action Index() được gọi khi yêu cầu truy cập đến URL tương ứng với http://[domain]/[controller]/Index.
        public ActionResult Index()
        {
            var items = _dal.GetAll<Category>();
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
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
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = _dal.GetById<Category>(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                _dal.Update(model);
                _dal.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dal.GetById<Category>(id);
            if (item != null)
            {
                _dal.Delete(item);
                _dal.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}