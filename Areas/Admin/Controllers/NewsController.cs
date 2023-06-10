using PagedList;
using ShopOnline.Data;
using ShopOnline.Models;
using ShopOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProject.Common;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class NewsController : Controller
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();
        // GET: Admin/News
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            var pageIndex = page ?? 1;

            IEnumerable<News> items = _dal.GetAll<News>().OrderByDescending(x => x.Id);

            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }

            var pagedItems = PageHelper.GetPagedList(items, page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = pagedItems.PageNumber;

            return View(pagedItems);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CategoryId = 3;
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
            var item = _dal.GetById<News>(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News model)
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
            var item = _dal.GetById<News>(id);
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
            var item = _dal.GetById<News>(id);
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
                    var entities = items.Select(item => _dal.GetById<News>(Convert.ToInt32(item))).ToList();
                    _dal.DeleteAll(entities);
                    _dal.SaveChanges();
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}