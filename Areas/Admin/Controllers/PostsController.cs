using ShopOnline.Data;
using ShopOnline.Models;
using ShopOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class PostsController : Controller
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();
        // GET: Admin/Posts
        public ActionResult Index()
        {
            var items = _dal.GetAll<Post>();
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Post model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CategoryId = 4;
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
            var item = _dal.GetById<Post>(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post model)
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
            var item = _dal.GetById<Post>(id);
            if (item != null)
            {
                _dal.Delete(item);
                _dal.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _dal.GetById<Post>(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _dal.Update(item);
                _dal.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
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
                    var entities = items.Select(item => _dal.GetById<Post>(Convert.ToInt32(item))).ToList();
                    _dal.DeleteAll(entities);
                    _dal.SaveChanges();
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}