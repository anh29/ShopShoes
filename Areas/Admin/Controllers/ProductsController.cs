using PagedList;
using ShopLearning.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ShopLearning.Models.ApplicationUser;

namespace ShopLearning.Areas.Admin.Controllers
{
    /*[Authorize(Roles = "Admin")]*/
    public class ProductsController : Controller
    {
        // GET: Admin/Products
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index( int? page)
        {
            var pagesize = 10;
            IEnumerable<Product> item = db.Product.OrderByDescending(x => x.Id);
            if (page==null)
            {
                page = 1;   
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            item = item.ToPagedList(pageIndex, pagesize);
            ViewBag.PageSize = pagesize;
            ViewBag.Page = page;
            return View(item);
        }
       public ActionResult Add()
        {       
                ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
                return View();      
        }
        [HttpPost]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault) 
        {
            if (ModelState.IsValid)
            {
                if(Images !=null && Images.Count > 0)
                {
                    for(int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            model.Image = Images[i];
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image= Images[i],
                                IsDefault=true
                            });

                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedrDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Title;
                }
                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = ShopLearning.Models.Common.Filter.FilterChar(model.Title);
                }
                db.Product.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        } 
        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(),"Id", "Title");
            var item = db.Product.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedrDate = DateTime.Now;
                model.Alias = ShopLearning.Models.Common.Filter.FilterChar(model.Title);
                db.Product.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var item = db.Product.Find(id);
            if (item != null)
            {
                db.Product.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Product.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsAcive = item.IsActive });

            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = db.Product.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });

            }
            return Json(new { success = false });
        }
        public ActionResult IsSale(int id)
        {
            var item = db.Product.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsSale = item.IsSale });

            }
            return Json(new { success = false });
        }
    }
}