using ShopOnline.Models;
using ShopOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace ShopOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        [Route("gio-hang")]
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        [Route("thanh-toan")]
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;

            }
            return View();
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.ModifiedDate = DateTime.Now;
                    order.CreatedBy = req.Phone;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    db.Orders.Add(order);
                    db.SaveChanges();
                    cart.ClearCart();
                    code = new { Success = true, Code = 1 };
                    return RedirectToAction("Index");

                }
            }
            return Json(code);
        }
        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return View(cart.Items);
            }
            return PartialView();
        }
        public ActionResult Partial_Item_CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(p => p.Id == id);
            if (checkProduct != null)
            {
                //truy xuất dữ liệu đã lưu trữ trong Session
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                // để lưu trữ thông tin phiên của người dùng trên máy chủ giữa các yêu cầu HTTP.
                Session["cart"] = cart;
                code = new { Success = true, msg = "Thêm sản phẩm thành công", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }

        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new {Success = true});
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                    cart.Remove(id);
                }
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new {Success = true});
            }
            return Json(new {Success = false});
        }
    }
}