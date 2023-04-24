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
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Category
        //Phương thức Action Index() được gọi khi yêu cầu truy cập đến URL tương ứng với http://[domain]/[controller]/Index.
        public ActionResult Index()
        {
            var items = db.Categories;
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
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = ShopOnline.Models.Common.Filter.FilterChar(model.Title);
                db.Categories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.Categories.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                //model là đối tượng Category cần được thực hiện các thao tác sửa đổi -> Attach để đính kèm đối tượng này vào context của database -> db.Entry để cập nhật dữ liệu
                db.Categories.Attach(model);
                model.ModifiedDate = DateTime.Now;
                model.Alias = ShopOnline.Models.Common.Filter.FilterChar(model.Title);
                db.Entry(model).Property(x => x.Title).IsModified = true; //IsModified -> để đánh dấu xem thuộc tính đó đã bị sửa đổi hay chưa.
                db.Entry(model).Property(x => x.Description).IsModified = true;
                db.Entry(model).Property(x => x.Alias).IsModified = true;
                db.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                db.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                db.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                db.Entry(model).Property(x => x.Position).IsModified = true;
                db.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                db.Entry(model).Property(x => x.Modifiedby).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Categories.Find(id);
            if (item != null)
            {
                db.Categories.Remove(item);
                db.SaveChanges();
                return Json(new {success=true});
            }
            return Json(new { success = false });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(Category model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Categories.Remove(model);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}

        //[HttpGet]
        //public ActionResult Delete(int? id, bool? saveChangesError = false)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    if (saveChangesError.GetValueOrDefault())
        //    {
        //        ViewBag.ErrorMessage = "Xóa danh mục thất bại. Vui lòng thử lại, hoặc liên hệ với quản trị viên.";
        //    }

        //    var item = db.Categories.Find(id.Value);

        //    if (item == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(item);
        //}
    }
}