using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ShopOnline.Models;
using ShopOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Data;
using ShopOnline.Domain;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace ShopOnline.Controllers
{
    public class CartManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly DataAccessLayer _dataAccessLayer;
        // GET: CartManager
        [Route("gio-hang")]
        public ActionResult Index()
        {
            CartManager cart = (CartManager)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        [Route("thanh-toan")]
        public ActionResult CheckOut()
        {
            CartManager cart = (CartManager)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;

            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOutOrder(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1 };
            if (ModelState.IsValid)
            {
                CartManager cart = (CartManager)Session["Cart"];
                if (cart != null && cart.Items.Any())
                {
                    Order order = new Order();
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = User.Identity.Name;
                        order.UserId = userId;
                    }
                    else
                    {
                        order.UserId = "No account";
                    }
                    order.CustomerName = req.CustomerName;
                    order.Email = req.Email;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    cart.Items.ForEach(x =>
                    {
                        // Add order details
                        order.OrderDetails.Add(new OrderDetail
                        {
                            ProductId = x.ProductId,
                            Quantity = x.Quantity,
                            Price = x.Product.Price
                        });

                        //Reduce product quantity
                        var product = db.Products.FirstOrDefault(p => p.Id == x.ProductId);
                        if (product != null)
                        {
                            if (product.Quantity >= x.Quantity)
                            {
                                product.Quantity -= x.Quantity;
                            }
                        }
                    }
                    );
                    order.TotalAmount = cart.Items.Sum(x => (x.Product.Price * x.Quantity));
                    order.TypePayment = req.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.ModifiedDate = DateTime.Now;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    db.Orders.Add(order);
                    db.SaveChanges();

                    //send mail cho khách hàng
                    var strSanPham = "";
                    var TongTien = decimal.Zero;
                    var ThanhTien = decimal.Zero;
                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.Product.Alias + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + ShopOnline.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                        strSanPham += "</tr>";
                        ThanhTien += sp.Product.Price * sp.Quantity;
                    }
                    TongTien = ThanhTien;

                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", order.CreatedDate.ToString());
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", ShopOnline.Common.Common.FormatNumber(ThanhTien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", ShopOnline.Common.Common.FormatNumber(TongTien, 0));
                    ShopOnline.Common.Common.SendMail("Shop TwinkleToes", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);
                    cart.ClearCart();
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult Partial_CheckOut()
        {
            var model = new OrderViewModel();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(userId);

                model.CustomerName = user.FullName;
                model.Phone = user.Phone;
                model.Email = user.Email;
            }

            return PartialView(model);
        }

        public ActionResult Partial_Item_Cart()
        {
            CartManager cart = null;
            if (User.Identity.IsAuthenticated)
            {

            }
            else
            {
                 cart = (CartManager)Session["Cart"];
            }
            if (cart != null && cart.Items.Any())
            {
                return View(cart.Items);
            }
            return PartialView();
        }
        public ActionResult Partial_Item_CheckOut()
        {
            CartManager cart = (CartManager)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            CartManager cart = (CartManager)Session["Cart"];
            if (cart != null && cart.Items.Any())
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
                CartManager cart = (CartManager)Session["Cart"];
                if (cart == null)
                {
                    cart = new CartManager();
                }
                CartItem item = new CartItem
                {
                    ProductId = checkProduct.Id,
                    Quantity = quantity
                };
                var Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * Price;
                cart.AddToCart(item, quantity);
                if (User.Identity.IsAuthenticated)
                {
                    _dataAccessLayer.Add(cart);
                }
                else
                {
                    // để lưu trữ thông tin phiên của người dùng trên máy chủ giữa các yêu cầu HTTP.
                    Session["cart"] = cart;
                }
                code = new { Success = true, msg = "Thêm sản phẩm thành công", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }

        public ActionResult Update(int id, int quantity)
        {
            CartManager cart = (CartManager)Session["cart"];
            if (cart != null && cart.Items.Any())
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            CartManager cart = (CartManager)Session["cart"];
            if (cart != null && cart.Items.Any())
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
            CartManager cart = (CartManager)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
    //public class CartManagerController : Controller
    //{
    //    private readonly DataAccessLayer _dataAccessLayer;

    //    public CartManagerController()
    //    {
    //        _dataAccessLayer = new DataAccessLayer();
    //    }

    //    // GET: CartManager
    //    [Route("gio-hang")]
    //    public ActionResult Index()
    //    {
    //        CartManager cart = GetCartFromSessionOrDatabase();
    //        if (cart != null && cart.Items.Any())
    //        {
    //            ViewBag.CheckCart = cart;
    //        }
    //        return View();
    //    }

    //    // Helper method to get cart from session or database based on user authentication
    //    public CartManager GetCartFromSessionOrDatabase()
    //    {
    //        CartManager cart = (CartManager)Session["Cart"];
    //        if (cart == null)
    //        {
    //            cart = new CartManager();
    //            Session["Cart"] = cart;
    //        }

    //        if (User.Identity.IsAuthenticated)
    //        {
    //            var userId = User.Identity.Name;
    //            var userCartItems = _dataAccessLayer.GetAll<CartItem>()
    //                .Where(c => c.Cart.UserId == userId)
    //                .ToList();

    //            // Add user's cart items to the session cart
    //            foreach (var item in userCartItems)
    //            {
    //                cart.AddToCart(item, item.Quantity);
    //            }
    //        }

    //        return cart;
    //    }

    //    [Route("thanh-toan")]
    //    public ActionResult CheckOut()
    //    {
    //        CartManager cart = GetCartFromSessionOrDatabase();
    //        if (cart != null && cart.Items.Any())
    //        {
    //            ViewBag.CheckCart = cart;
    //        }
    //        return View();
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult CheckOutOrder(OrderViewModel req)
    //    {
    //        var code = new { Success = false, Code = -1 };
    //        if (ModelState.IsValid)
    //        {
    //            CartManager cart = (CartManager)Session["Cart"];
    //            if (cart != null && cart.Items.Any())
    //            {
    //                Order order = new Order();
    //                if (User.Identity.IsAuthenticated)
    //                {
    //                    var userId = User.Identity.Name;
    //                    order.UserId = userId;
    //                }
    //                else
    //                {
    //                    order.UserId = "No account";
    //                }
    //                order.CustomerName = req.CustomerName;
    //                order.Email = req.Email;
    //                order.Phone = req.Phone;
    //                order.Address = req.Address;

    //                foreach (var cartItem in cart.Items)
    //                {
    //                    var product = _dataAccessLayer.GetById<Product>(cartItem.ProductId);
    //                    if (product != null && product.Quantity >= cartItem.Quantity)
    //                    {
    //                        // Add order details
    //                        order.OrderDetails.Add(new OrderDetail
    //                        {
    //                            ProductId = cartItem.ProductId,
    //                            Quantity = cartItem.Quantity,
    //                            Price = cartItem.Product.Price
    //                        });

    //                        // Reduce product quantity
    //                        _dataAccessLayer.ReduceProductQuantity(cartItem.ProductId, cartItem.Quantity);
    //                    }
    //                }

    //                order.TotalAmount = cart.Items.Sum(x => (x.Product.Price * x.Quantity));
    //                order.TypePayment = req.TypePayment;
    //                order.CreatedDate = DateTime.Now;
    //                order.ModifiedDate = DateTime.Now;
    //                Random rd = new Random();
    //                order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);

    //                _dataAccessLayer.Add(order);
    //                _dataAccessLayer.SaveChanges();

    //                //send mail cho khách hàng
    //                var strSanPham = "";
    //                var TongTien = decimal.Zero;
    //                var ThanhTien = decimal.Zero;
    //                foreach (var sp in cart.Items)
    //                {
    //                    strSanPham += "<tr>";
    //                    strSanPham += "<td>" + sp.Product.Alias + "</td>";
    //                    strSanPham += "<td>" + sp.Quantity + "</td>";
    //                    strSanPham += "<td>" + ShopOnline.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
    //                    strSanPham += "</tr>";
    //                    ThanhTien += sp.Product.Price * sp.Quantity;
    //                }
    //                TongTien = ThanhTien;

    //                string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("/Content/templates/send2.html"));
    //                contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
    //                contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
    //                contentCustomer = contentCustomer.Replace("{{NgayDat}}", order.CreatedDate.ToString());
    //                contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
    //                contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
    //                contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
    //                contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
    //                contentCustomer = contentCustomer.Replace("{{ThanhTien}}", ShopOnline.Common.Common.FormatNumber(ThanhTien, 0));
    //                contentCustomer = contentCustomer.Replace("{{TongTien}}", ShopOnline.Common.Common.FormatNumber(TongTien, 0));
    //                ShopOnline.Common.Common.SendMail("Shop TwinkleToes", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);
    //                cart.ClearCart();
    //                return RedirectToAction("CheckOutSuccess");
    //            }
    //        }
    //        return Json(code);
    //    }
    //    public ActionResult CheckOutSuccess()
    //    {
    //        return View();
    //    }
    //    public ActionResult Partial_CheckOut()
    //    {
    //        var model = new OrderViewModel();

    //        if (User.Identity.IsAuthenticated)
    //        {
    //            var userId = User.Identity.GetUserId();
    //            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
    //            var user = userManager.FindById(userId);

    //            model.CustomerName = user.FullName;
    //            model.Phone = user.Phone;
    //            model.Address = user.Address;
    //            model.Email = user.Email;
    //        }

    //        return PartialView(model);
    //    }

    //    public ActionResult Partial_Item_Cart()
    //    {
    //        CartManager cart = (CartManager)Session["Cart"];
    //        if (cart != null && cart.Items.Any())
    //        {
    //            return PartialView(cart.Items);
    //        }
    //        return PartialView();
    //    }
    //    public ActionResult Partial_Item_CheckOut()
    //    {
    //        CartManager cart = (CartManager)Session["Cart"];
    //        if (cart != null && cart.Items.Any())
    //        {
    //            return PartialView(cart.Items);
    //        }
    //        return PartialView();
    //    }
    //    public ActionResult ShowCount()
    //    {
    //        CartManager cart = null;

    //        // Check if the user is logged in
    //        if (User.Identity.IsAuthenticated)
    //        {
    //            // Retrieve the user's cart from the database using their user ID
    //            var userId = User.Identity.GetUserId();
    //            cart = _dataAccessLayer.GetById<CartManager>(userId);
    //        }
    //        else
    //        {
    //            // Retrieve the cart from the session
    //            cart = (CartManager)Session["Cart"];
    //        }

    //        if (cart != null && cart.Items.Any())
    //        {
    //            return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
    //        }

    //        return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
    //    }


    //    [HttpPost]
    //    public ActionResult AddToCart(int id, int quantity)
    //    {
    //        var code = new { Success = false, msg = "", code = -1, Count = 0 };
    //        var checkProduct = _dataAccessLayer.GetById<Product>(id);
    //        if (checkProduct != null)
    //        {
    //            CartManager cart;

    //            if (User.Identity.IsAuthenticated)
    //            {
    //                var userId = User.Identity.Name;

    //                // Check if the cart item exists in the database for the logged-in user
    //                var cartItem = _dataAccessLayer.GetAll<CartItem>().FirstOrDefault(c => c.Cart.UserId == userId && c.ProductId == id);

    //                if (cartItem != null)
    //                {
    //                    // Update the existing cart item quantity in the database
    //                    cartItem.Quantity += quantity;
    //                    cartItem.TotalPrice = cartItem.Product.Price * cartItem.Quantity;
    //                    _dataAccessLayer.Update(cartItem);
    //                }
    //                else
    //                {
    //                    // Create a new cart item and save it to the database
    //                    cartItem = new CartItem
    //                    {
    //                        ProductId = checkProduct.Id,
    //                        Quantity = quantity
    //                    };

    //                    if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
    //                    {
    //                        cartItem.Product.Image = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
    //                    }

    //                    cartItem.Product.Price = checkProduct.Price;
    //                    if (checkProduct.PriceSale > 0)
    //                    {
    //                        cartItem.Product.Price = (decimal)checkProduct.PriceSale;
    //                    }
    //                    cartItem.TotalPrice = cartItem.Quantity * cartItem.Product.Price;

    //                    _dataAccessLayer.Add(cartItem);
    //                }

    //                _dataAccessLayer.SaveChanges();

    //                // Retrieve the user's cart items from the database
    //                var userCartItems = _dataAccessLayer.GetAll<CartItem>().Where(c => c.Cart.UserId == userId).ToList();
    //                cart = new CartManager();
    //                foreach (var item in userCartItems)
    //                {
    //                    cart.AddToCart(item, item.Quantity);
    //                }
    //            }
    //            else
    //            {
    //                // Retrieve the cart from the session
    //                cart = (CartManager)Session["Cart"];
    //                if (cart == null)
    //                {
    //                    cart = new CartManager();
    //                }

    //                CartItem item = new CartItem
    //                {
    //                    ProductId = checkProduct.Id,
    //                    Quantity = quantity
    //                };

    //                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
    //                {
    //                    item.Product.Image = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
    //                }

    //                item.Product.Price = checkProduct.Price;
    //                if (checkProduct.PriceSale > 0)
    //                {
    //                    item.Product.Price = (decimal)checkProduct.PriceSale;
    //                }
    //                item.TotalPrice = item.Quantity * item.Product.Price;

    //                cart.AddToCart(item, quantity);

    //                // Store the cart in the session
    //                Session["Cart"] = cart;
    //            }

    //            code = new { Success = true, msg = "Thêm sản phẩm thành công", code = 1, Count = cart.Items.Count };
    //        }

    //        return Json(code);
    //    }

    //    public ActionResult Update(int id, int quantity)
    //    {
    //        if (User.Identity.IsAuthenticated)
    //        {
    //            var userId = User.Identity.Name;

    //            // Check if the cart item exists in the database for the logged-in user
    //            var cartItem = _dataAccessLayer.GetAll<CartItem>().FirstOrDefault(c => c.Cart.UserId == userId && c.ProductId == id);

    //            if (cartItem != null)
    //            {
    //                cartItem.Quantity = quantity;
    //                cartItem.TotalPrice = cartItem.Product.Price * cartItem.Quantity;
    //                _dataAccessLayer.Update(cartItem);
    //                _dataAccessLayer.SaveChanges();
    //            }
    //        }
    //        else
    //        {
    //            CartManager cart = (CartManager)Session["Cart"];
    //            if (cart != null && cart.Items.Any())
    //            {
    //                cart.UpdateQuantity(id, quantity);
    //            }
    //        }

    //        return Json(new { Success = true });
    //    }

    //    [HttpPost]
    //    public ActionResult Delete(int id)
    //    {
    //        CartManager cart = null;

    //        if (User.Identity.IsAuthenticated)
    //        {
    //            var userId = User.Identity.Name;

    //            // Check if the cart item exists in the database for the logged-in user
    //            var cartItem = _dataAccessLayer.GetAll<CartItem>().FirstOrDefault(c => c.Cart.UserId == userId && c.ProductId == id);

    //            if (cartItem != null)
    //            {
    //                _dataAccessLayer.Delete(cartItem);
    //                _dataAccessLayer.SaveChanges();
    //            }
    //        }
    //        else
    //        {
    //            cart = (CartManager)Session["Cart"];
    //            if (cart != null && cart.Items.Any())
    //            {
    //                cart.Remove(id);
    //            }
    //        }

    //        var code = new { Success = true, msg = "", code = 1, Count = cart?.Items.Count ?? 0 };
    //        return Json(code);
    //    }

    //    [HttpPost]
    //    public ActionResult DeleteAll()
    //    {
    //        CartManager cart = null;

    //        if (User.Identity.IsAuthenticated)
    //        {
    //            var userId = User.Identity.Name;

    //            // Retrieve the cart items for the logged-in user
    //            var cartItems = _dataAccessLayer.GetAll<CartItem>().Where(c => c.Cart.UserId == userId).ToList();

    //            if (cartItems.Any())
    //            {
    //                foreach (var cartItem in cartItems)
    //                {
    //                    _dataAccessLayer.Delete(cartItem);
    //                }

    //                _dataAccessLayer.SaveChanges();
    //            }
    //        }
    //        else
    //        {
    //            cart = (CartManager)Session["Cart"];
    //            if (cart != null && cart.Items.Any())
    //            {
    //                cart.ClearCart();
    //            }
    //        }

    //        return Json(new { Success = true });
    //    }

    //}
}