using PagedList;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var items = db.Orders.OrderByDescending(x => x.CreatedDate).ToList();

            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(items.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Detail(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham(int? id)
        {
            var item = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(item);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.TypePayment= status;
                db.Entry(item).Property(x=>x.TypePayment).IsModified = true;
                db.SaveChanges();
                return Json(new {message = "Success", Success = true});
            }
            return Json(new {message = "Fail", Success = false});
        }
    }
}