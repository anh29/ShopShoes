using PagedList;
using ShopOnline.Data;
using ShopOnline.Models;
using ShopOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using YourProject.Common;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();

        // GET: Admin/Order
        public ActionResult Index(string Searchtext, int? page)
        {
            IEnumerable<Order> items = _dal.GetAll<Order>().OrderByDescending(x => x.CreatedDate);

            var pageNumber = page ?? 1;
            var pageSize = 10;

            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.CustomerName.ToLower().Contains(Searchtext.ToLower()) || x.UserId.ToLower().Contains(Searchtext.ToLower()));
            }

            var pagedItems = PageHelper.GetPagedList(items, page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;

            return View(pagedItems);
        }

        public ActionResult Detail(int id)
        {
            var item = _dal.GetById<Order>(id);
            return View(item);
        }
        public ActionResult Partial_SanPham(int? id)
        {
            var item = _dal.GetAll<OrderDetail>().Where(x => x.OrderId == id).ToList();
            return PartialView(item);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            var item = _dal.GetById<Order>(id);
            if (item != null)
            {
                item.State = status;
                _dal.Update(item);
                _dal.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Fail", Success = false });
        }
    }
    }