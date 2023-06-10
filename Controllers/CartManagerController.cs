using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ShopOnline.Models.EF;
using ShopOnline.Models;
using System.Web.Mvc;
using System;
using ShopOnline.Domain;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.Owin.Logging;
using ShopOnline.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

public class CartManagerController : Controller
{
    private readonly DataAccessLayer _dal = new DataAccessLayer();
    // GET: CartManager
    [Route("gio-hang")]
    public ActionResult Index()
    {
        return View();
    }

    [Route("thanh-toan")]
    public ActionResult CheckOut()
    {
        CartManager cart = null;
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.Identity.Name;
            var cartdb = _dal.GetFiltered<Cart>(c => c.UserId == userId).SingleOrDefault();

            if (cartdb != null)
            {
                var cartItems = _dal.GetFiltered<CartItem>(ci => ci.CartId == cartdb.Id).ToList();
                ViewBag.CheckCart = cartItems;
            }
        }
        else
        {
            cart = (CartManager)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
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
            List<CartItem> cartItems = null;
            var user = User.Identity.IsAuthenticated;
            if (user)
            {
                var userId = User.Identity.Name;
                var cart = _dal.GetAllWithInclude<Cart>(c => c.CartItems)
                    .SingleOrDefault(c => c.UserId == userId);
                if (cart != null && cart.CartItems.Any())
                {
                    cartItems = cart.CartItems.ToList();
                }
            }
            else
            {
                var cartManager = (CartManager)Session["Cart"];
                if (cartManager != null && cartManager.Items.Any())
                {
                    cartItems = cartManager.Items.ToList();
                }
            }

            if (cartItems.Any())
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
                order.Note = req.Note;
                order.Address = req.Address;
                cartItems.ForEach(x =>
                {
                    //Reduce product quantity
                    var product = _dal.GetById<Product>(x.ProductId);
                    var price = product.Price;
                    if (product != null)
                    {
                        if (product.Quantity >= x.Quantity)
                        {
                            product.Quantity -= x.Quantity;
                            _dal.Update(product);
                        }
                    }
                    if (product.PriceSale > 0)
                    {
                        price = (decimal)product.PriceSale;
                    }
                    // Add order details
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = price
                    });
                });
                order.TotalAmount = cartItems.Sum(x => (x.TotalPrice));
                order.State = req.TypePayment;
                order.CreatedDate = DateTime.Now;
                order.ModifiedDate = DateTime.Now;
                Random rd = new Random();
                order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                _dal.Add(order);
                _dal.SaveChanges();

                //send mail cho khách hàng
                var strSanPham = "";
                var TongTien = decimal.Zero;
                var Sale = decimal.Zero;
                var ThanhTien = decimal.Zero;
                foreach (var sp in cartItems)
                {
                    strSanPham += "<tr>";
                    strSanPham += "<td>" + sp.Product.Title + "</td>";
                    strSanPham += "<td>" + ShopOnline.Common.Common.FormatNumber(sp.Product.Price, 0) + "</td>";
                    var price = sp.Product.Price;
                    if (sp.Product.PriceSale > 0)
                    {
                        strSanPham += "<td>" + ShopOnline.Common.Common.FormatNumber(sp.Product.PriceSale, 0) + "</td>";
                        price = (decimal)sp.Product.PriceSale;
                        Sale = (decimal)(Sale + sp.Product.Price - price);
                    }
                    else
                    {
                        strSanPham += "<td> 0 </td>";
                    }
                    strSanPham += "<td>" + sp.Quantity + "</td>";
                    strSanPham += "<td>" + ShopOnline.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                    strSanPham += "</tr>";
                    ThanhTien += price * sp.Quantity;
                }
                TongTien = ThanhTien;

                string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("/Content/templates/sendOrder.html"));
                contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                contentCustomer = contentCustomer.Replace("{{NgayDat}}", order.CreatedDate.ToString());
                contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                contentCustomer = contentCustomer.Replace("{{Note}}", order.Note);
                contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                contentCustomer = contentCustomer.Replace("{{Sale}}", ShopOnline.Common.Common.FormatNumber(Sale, 0));
                contentCustomer = contentCustomer.Replace("{{TongTien}}", ShopOnline.Common.Common.FormatNumber(TongTien, 0));
                ShopOnline.Common.Common.SendMail("Shop TwinkleToes", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);
                if (user)
                {
                    cartItems.ForEach(x => _dal.Delete(x));
                    _dal.SaveChanges();
                }
                else
                {
                    cartItems.Clear();
                }
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
            model.Phone = user.PhoneNumber;
            model.Address = user.Address;
            model.Email = user.Email;
        }

        return PartialView(model);
    }

    public ActionResult Partial_Item_Cart()
    {
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.Identity.Name;
            var cart = _dal.GetAllWithInclude<Cart>(c => c.CartItems)
                    .SingleOrDefault(c => c.UserId == userId);
            if (cart != null && cart.CartItems.Any())
            {
                var cartItems = cart.CartItems.ToList();
                return View(cartItems);
            }
        }
        else
        {
            var cartManager = (CartManager)Session["Cart"];
            if (cartManager != null && cartManager.Items.Any())
            {
                var cartItems = cartManager.Items.ToList();
                return View(cartItems);
            }
        }
        return PartialView();
    }

    public ActionResult Partial_Item_CheckOut()
    {
        List<CartItem> items = null;
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.Identity.Name;
            var cart = _dal.GetAllWithInclude<Cart>(c => c.CartItems)
                    .SingleOrDefault(c => c.UserId == userId);
            if (cart != null && cart.CartItems.Any())
            {
                items = cart.CartItems.ToList();
            }
        }
        else
        {
            var cartManager = (CartManager)Session["Cart"];
            if (cartManager != null && cartManager.Items.Any())
            {
                items = cartManager.Items.ToList();
            }
        }
        if (items.Any())
        {
            return PartialView(items);
        }
        return PartialView();
    }
    public ActionResult ShowCount()
    {
        if (User.Identity.IsAuthenticated)
        {
            var cart = _dal.GetFiltered<Cart>(c => c.UserId == User.Identity.Name).SingleOrDefault();
            if (cart != null)
            {
                var count = _dal.GetFiltered<CartItem>(i => i.CartId == cart.Id)
                                .Select(i => i.ProductId)
                                .Distinct()
                                .Count();
                return Json(new { Count = count }, JsonRequestBehavior.AllowGet);
            }
        }
        else
        {
            CartManager cart = (CartManager)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
        }

        return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
    }
    [HttpPost]
    public ActionResult AddToCart(int id, int quantity)
    {
        var code = new { Success = false, msg = "", code = -1, Count = 0 };
        var user = User.Identity.IsAuthenticated;

        var product = _dal.GetById<Product>(id);

        if (product != null)
        {
            Cart cart = null;
            if (user)
            {
                // Get the cart for the logged-in user
                cart = _dal.GetFiltered<Cart>(c => c.UserId == User.Identity.Name).SingleOrDefault();
                if (cart == null)
                {
                    // If the user does not have a cart yet, create a new one
                    cart = new Cart { UserId = User.Identity.Name };
                    _dal.Add(cart);
                    _dal.SaveChanges();
                }
            }
            else
            {
                // Get the cart from the session for the anonymous user
                var cartManager = (CartManager)Session["Cart"];
                if (cartManager == null)
                {
                    // If the cartManager is null, create a new one
                    cartManager = new CartManager();
                }
                cart = new Cart { CartItems = new List<CartItem>() }; // Create a new cart
            }

            // Check if the product already exists in the cart
            var existingCartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == product.Id);
            if (existingCartItem != null)
            {
                // If the product already exists, increase the quantity
                existingCartItem.Quantity += quantity;
                existingCartItem.TotalPrice = existingCartItem.Quantity * (product.PriceSale > 0 ? (decimal)product.PriceSale : product.Price);
            }
            else
            {
                // If the product does not exist, add it to the cart
                cart.CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    TotalPrice = quantity * (product.PriceSale > 0 ? (decimal)product.PriceSale : product.Price),
                    Product = product
                });
            }

            if (user)
            {
                _dal.SaveChanges();
                code = new
                {
                    Success = true,
                    msg = "Thêm sản phẩm thành công",
                    code = 1,
                    Count = cart.CartItems.Count()
                };
            }
            else
            {
                // Add the cart item to the cart in the session for the anonymous user
                var cartManager = (CartManager)Session["Cart"];
                if (cartManager == null)
                {
                    cartManager = new CartManager();
                }
                cartManager.AddToCart(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    TotalPrice = quantity * (product.PriceSale > 0 ? (decimal)product.PriceSale : product.Price),
                    Product = product
                }, quantity);
                Session["Cart"] = cartManager;
                code = new
                {
                    Success = true,
                    msg = "Thêm sản phẩm thành công",
                    code = 1,
                    Count = cartManager.Items.Count
                };
            }

        }

        return Json(code);
    }

    public ActionResult Update(int id, int quantity)
    {
        CartManager cart = null;
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.Identity.Name;
            var cartdb = _dal.GetAllWithInclude<Cart>(c => c.CartItems)
                .SingleOrDefault(c => c.UserId == userId);
            if (cartdb != null && cartdb.CartItems.Any())
            {
                var cartItem = cartdb.CartItems.FirstOrDefault(ci => ci.ProductId == id);
                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    cartItem.TotalPrice = cartItem.Quantity * (cartItem.Product.PriceSale > 0 ? (decimal)cartItem.Product.PriceSale : cartItem.Product.Price);
                    _dal.SaveChanges();
                    return Json(new { Success = true });
                }
            }
        }
        else
        {
            cart = (CartManager)Session["cart"];
        }
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
        CartManager cart = null;
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.Identity.Name;
            var cartdb = _dal.GetAllWithInclude<Cart>(c => c.CartItems)
                .SingleOrDefault(c => c.UserId == userId);
            if (cartdb != null && cartdb.CartItems.Any())
            {
                var cartItem = cartdb.CartItems.FirstOrDefault(ci => ci.ProductId == id);
                if (cartItem != null)
                {
                    var count = cartdb.CartItems.Select(i => i.ProductId).Distinct().Count();
                    code = new { Success = true, msg = "", code = 1, Count = count };
                    _dal.Delete(cartItem);
                    _dal.SaveChanges();
                }
            }
        }
        else
        {
            cart = (CartManager)Session["cart"];
            if (cart != null && cart.Items.Any())
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                    cart.Remove(id);
                }
            }
        }
        return Json(code);
    }

    [HttpPost]
    public ActionResult DeleteAll()
    {
        List<CartItem> cartItems = null;
        var user = User.Identity.IsAuthenticated;
        if (user)
        {
            var userId = User.Identity.Name;
            var cart = _dal.GetAllWithInclude<Cart>(c => c.CartItems)
                .SingleOrDefault(c => c.UserId == userId);
            if (cart != null && cart.CartItems.Any())
            {
                cartItems = cart.CartItems.ToList();
                cartItems.ForEach(x => _dal.Delete(x));
                _dal.SaveChanges();
                return Json(new { Success = true, Count = 0 });
            }
        }
        else
        {
            var cartManager = (CartManager)Session["Cart"];
            if (cartManager != null && cartManager.Items.Any())
            {
                cartManager.Items.Clear(); // xóa các phần tử trong danh sách của cartManager
                cartItems = cartManager.Items.ToList();
                cartItems.Clear(); // xóa các phần tử trong danh sách cartItems
                Session["Cart"] = cartManager; // cập nhật lại đối tượng CartManager trong session
                return Json(new { Success = true });
            }
        }
        return Json(new { Success = false, Count = 0 });
    }
}