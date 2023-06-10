using PagedList;
using ShopOnline.Data;
using ShopOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProject.Common;

namespace ShopOnline.Controllers
{
    public class ArticleController : Controller
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();

        // GET: Article
        [Route("bai-viet")]
        public ActionResult Index(int? page)
        {
            var pageSize = 5;
            var items = _dal.GetAll<Post>().OrderByDescending(x => x.Id);
            var pagedItems = PageHelper.GetPagedList(items, page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = pagedItems.PageNumber;
            return View(pagedItems);
        }

        [Route("bai-viet/{alias}-p{id}")]
        public ActionResult Detail(int id)
        {
            var item = _dal.GetById<Post>(id);
            return View(item);
        }

        public ActionResult Partial_Post_Home()
        {
            var items = _dal.GetAll<Post>().Take(3).ToList();
            return PartialView(items);
        }
    }
}
