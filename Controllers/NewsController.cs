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

namespace ShopOnline.Controllers
{
    public class NewsController : Controller
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();

        [Route("tin-tuc")]
        // GET: News
        public ActionResult Index(int? page)
        {
            IEnumerable<News> items = _dal.GetAll<News>().OrderByDescending(x => x.CreatedDate);

            var pageNumber = page ?? 1;
            var pageSize = 10;

            var pagedItems = PageHelper.GetPagedList(items, page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;

            return View(pagedItems);
        }

        [Route("{alias}-n{id}")]
        public ActionResult Detail(int id)
        {
            var item = _dal.GetById<News>(id);
            return View(item);
        }

        public ActionResult Partial_News_Home()
        {
            var items = _dal.GetAll<News>().Take(3).ToList();
            return PartialView(items);
        }
    }
}
