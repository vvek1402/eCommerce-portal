using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Models;
using System.Web.Script.Serialization;

namespace E_Commerce.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        Entities1 db = new Entities1();
        public ActionResult home()
        {
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                HomeViewModel homemodel = new HomeViewModel();
                homemodel.ProductList = db.products.ToList();
                homemodel.ProductVariantList = db.product_variant_value.ToList();
                homemodel.ProductImageList = db.images.ToList();
                homemodel.categoryList = db.categories.ToList();
                return View(homemodel);
            }
        }

        public ActionResult cart()
        {
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                CartViewModel cartvm = new CartViewModel();
                long uid = Convert.ToInt64(Session["user_id"]);
                cartvm.cartList = db.carts.Where(x => x.user_id == uid).ToList();
                cartvm.GrandTotal = 0;
                cartvm.productVariantList = db.product_variant_value.ToList();
                cartvm.productImageList = db.images.ToList();
                return View(cartvm);
            }
              
        }

        [HttpPost]
        public ActionResult removecartitem(long id)
        {
            cart c = db.carts.Where(x => x.cart_id == id).SingleOrDefault();
            db.carts.Remove(c);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { IsValid = true, Success = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return RedirectToAction("cart", "Home", new { area = "User" });
            }
        }

        public JsonResult savecartqty(string id,int qty)
        {
            long cid = Convert.ToInt64(id);
            cart c = db.carts.Where(x => x.cart_id == cid).SingleOrDefault();
            c.qty = qty;
            db.SaveChanges();
            return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
        }
        public ActionResult getcartList()
        {

            CartViewModel cartvm = new CartViewModel();
            long uid = Convert.ToInt64(Session["user_id"]);
            cartvm.cartList = db.carts.Where(x => x.user_id == uid).ToList();
            cartvm.GrandTotal = 0;
            cartvm.productVariantList = db.product_variant_value.ToList();
            cartvm.productImageList = db.images.ToList();
            return PartialView("carttable", cartvm);
        }

        public JsonResult getcartitemno()
        {
            long uid = Convert.ToInt64(Session["user_id"]);
            int count = db.carts.Where(x => x.user_id == uid).ToList().Count;
            return Json(new { data=count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult shop()
        {
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                ShopViewModel shop = new ShopViewModel();
                shop.categoryList = db.categories.ToList();
                shop.subcategoryList = db.sub_category.ToList();
                shop.productList = db.products.ToList();
                shop.productvariantList = db.product_variant_value.ToList();
                shop.productimageList = db.images.ToList();
                return View(shop);
            }
        }
        public ActionResult productdetail(string id)
        {
            
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                long pid =0;
                try
                {
                    pid = Convert.ToInt64(id);
                    HomeViewModel homemodel = new HomeViewModel();
                    homemodel.Product = db.products.Where(x => x.product_id == pid).SingleOrDefault();
                    homemodel.ProductVariantList = db.product_variant_value.Where(x => x.product_id == pid).ToList();
                    homemodel.ProductImageList = db.images.ToList();
                    homemodel.category = db.categories.Where(x => x.category_id == homemodel.Product.category_id).SingleOrDefault();
                    homemodel.sub_category = db.sub_category.Where(x => x.sub_category_id == homemodel.Product.sub_category_id).SingleOrDefault();
                    homemodel.variant_value = db.variant_value.ToList();
                    if (homemodel.Product.isvariant == 1)
                    {
                        Tuple<List<variant_value>, List<string>> data = getList(homemodel.ProductVariantList);
                        homemodel.variantList = data.Item1;
                        homemodel.vnameList = data.Item2;
                    }
                    homemodel.reviewList = db.reviews.ToList();
                    homemodel.userList = db.User_account.ToList();

                    return View(homemodel);
                }
                catch
                {
                    return null;
                }
                
            }
        }

        

        private Tuple<List<variant_value>, List<string>> getList(List<product_variant_value> pvl)
        {
           
                List<variant_value> vvalue = new List<variant_value>();
                foreach (product_variant_value pv in pvl)
                    vvalue.Add(db.variant_value.Where(x => x.variant_value_id == pv.variant_value_id).SingleOrDefault());
                long id = vvalue[0].variant_id;
                variant v = db.variants.Where(x => x.variant_id == id).SingleOrDefault();
                List<string> vname = v.variant_name.Split('_').ToList();

                Tuple<List<variant_value>, List<string>> re = Tuple.Create(vvalue, vname);

                return re;
            
        }

        public JsonResult GetVariantData(string id)
        {   
            long vvid=Convert.ToInt64(id);
            product_variant_value pv = db.product_variant_value.Where(x => x.variant_value_id == vvid).SingleOrDefault();
            long pvid = pv.product_variant_id;
            image i = db.images.Where(x => x.product_variant_id == pvid).SingleOrDefault();
            int count = 0;
            if (i.image1 != "") count++;
            if (i.image2 != "") count++;
            if (i.image3 != "") count++;
            if (i.image4 != "") count++;
            return Json(new { main_image = i.main_image,
                              image1 = i.image1,
                              image2 = i.image2,
                              image3 = i.image3,
                              image4 = i.image4,
                              title=pv.product_variant_title,
                              MRP=pv.MRP,
                              sprice=pv.Selling_price,
                              stock=pv.unit_in_stock,
                              sku=pv.sku,
                              weight=pv.weight,
                              qpu=pv.quantity_per_unit,
                              pvid=pvid,
                            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult addcart(string id)
        {
            
            long pvid = long.Parse(id);
            long uid = Convert.ToInt64(Session["user_id"]);
            cart c = db.carts.Where(x => x.product_variant_id == pvid && x.user_id == uid).SingleOrDefault();
            if (c == null)
            {
                cart cart = new cart();

                cart.product_variant_id = pvid;
                cart.user_id = uid;
                cart.qty = 1;
                db.carts.Add(cart);
                db.SaveChanges();
            }
            else
            {
                c.qty = c.qty + 1;
                db.SaveChanges();

            }
            return Json(new { result = true }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult tracking(string id)
        {
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                long oid = Convert.ToInt64(id);
                OrderModel ordermodel = new OrderModel();


                ordermodel.order = db.Orders.Where(x => x.order_id == oid).SingleOrDefault();
                ordermodel.StatusList = db.order_status.ToList();
                ordermodel.users = db.User_account.ToList();
                User_account admin = db.User_account.Where(x => x.role_id == 1).SingleOrDefault();
                ordermodel.adminadd = db.User_address.Where(x => x.user_id == admin.user_id && x.isdefault == 1).SingleOrDefault();

                ordermodel.cities = db.cities.ToList();

                ordermodel.countries = db.Countries.ToList();
                ordermodel.OrderDetailList = db.Order_detail.ToList();
                ordermodel.ProductVariantList = db.product_variant_value.ToList();
                ordermodel.imageList = db.images.ToList();
                ViewBag.StatusList = new SelectList(ordermodel.StatusList, "status_id", "status");

                return View(ordermodel);
            }
        }
        public ActionResult printcustinvoice(int id)
        {
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                OrderModel ordermodel = new OrderModel();
                long order_id = Convert.ToInt64(id);

                User_account admin = db.User_account.Where(x => x.role_id == 1).SingleOrDefault();
                ordermodel.users = db.User_account.ToList();
                ordermodel.order = db.Orders.Where(x => x.order_id == order_id).SingleOrDefault();
                ordermodel.cities = db.cities.ToList();
                ordermodel.countries = db.Countries.ToList();
                ordermodel.OrderDetailList = db.Order_detail.Where(x => x.order_id == order_id).ToList();
                ordermodel.badd = db.User_address.Where(x => x.user_add_id == ordermodel.order.billing_add).SingleOrDefault();
                ordermodel.sadd = db.User_address.Where(x => x.user_add_id == ordermodel.order.shipping_add).SingleOrDefault();
                ordermodel.adminadd = db.User_address.Where(x => x.user_id == admin.user_id && x.isdefault == 1).SingleOrDefault();
                ordermodel.ProductVariantList = db.product_variant_value.ToList();

                return View(ordermodel);
            }
        }
        public ActionResult search(string searchinput, string sortOrder,string rating)
        {
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                SeachViewModel model = new SeachViewModel();
                model.productList = db.product_variant_value.Where(x => x.product_variant_title.Contains(searchinput)).ToList();
                model.imageList = db.images.ToList();

                List<category> clist = db.categories.Where(x => x.category_name.Contains(searchinput)).ToList();
                foreach (category c in clist)
                {
                    List<product> productList = db.products.Where(x => x.category_id == c.category_id).ToList();
                    foreach (product p in productList)
                    {
                        List<product_variant_value> pvlist = db.product_variant_value.Where(x => x.product_id == p.product_id).ToList();
                        foreach (product_variant_value pv in pvlist)
                        {
                            model.productList.Add(pv);
                        }
                    }
                }

                List<sub_category> sclist = db.sub_category.Where(x => x.sub_category_name.Contains(searchinput)).ToList();
                foreach (sub_category c in sclist)
                {
                    List<product> productList = db.products.Where(x => x.sub_category_id == c.sub_category_id).ToList();
                    foreach (product p in productList)
                    {
                        List<product_variant_value> pvlist = db.product_variant_value.Where(x => x.product_id == p.product_id).ToList();
                        foreach (product_variant_value pv in pvlist)
                        {
                            model.productList.Add(pv);
                        }
                    }
                }

                if (sortOrder == "ltoh")
                    model.productList = model.productList.OrderBy(x => x.Selling_price).ToList();
                else if (sortOrder == "htol")
                    model.productList = model.productList.OrderByDescending(x => x.Selling_price).ToList();

                if (rating != null)
                {
                    int ratingvalue = Convert.ToInt32(rating);
                    List<product_variant_value> tempList = model.productList.ToList();
                    model.productList.Clear();
                    foreach (product_variant_value pv in tempList)
                    {
                        product p = db.products.Where(x => x.product_id == pv.product_id).SingleOrDefault();
                        if (p.rating >= ratingvalue)
                            model.productList.Add(pv);
                    }
                }
                return View(model);
            }
        }

        public ActionResult wishlist()
        {
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                CartViewModel wishlistvm = new CartViewModel();
                long uid = Convert.ToInt64(Session["user_id"]);
                wishlistvm.wishlistList = db.wishlists.Where(x => x.user_id == uid).ToList();

                wishlistvm.productVariantList = db.product_variant_value.ToList();
                wishlistvm.productImageList = db.images.ToList();
                return View(wishlistvm);
            }

        }

        public JsonResult getwishlistitemno()
        {
            long uid = Convert.ToInt64(Session["user_id"]);
            int count = db.wishlists.Where(x => x.user_id == uid).ToList().Count;
            return Json(new { data = count }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addwishlist(string id)
        {

            long pvid = long.Parse(id);
            long uid = Convert.ToInt64(Session["user_id"]);
            wishlist wishlist = new wishlist();
            wishlist.product_variant_id = pvid;
            wishlist.user_id = uid;

            db.wishlists.Add(wishlist);
            db.SaveChanges();

            return Json(new { result = true }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult removewishlistitem(long id)
        {
            wishlist c = db.wishlists.Where(x => x.wishlist_id == id).SingleOrDefault();
            db.wishlists.Remove(c);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { IsValid = true, Success = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return RedirectToAction("wishlist", "Home", new { area = "User" });
            }
        }
        public ActionResult getwishlistList()
        {
            CartViewModel wishlistvm = new CartViewModel();
            long uid = Convert.ToInt64(Session["user_id"]);
            wishlistvm.wishlistList = db.wishlists.Where(x => x.user_id == uid).ToList();

            wishlistvm.productVariantList = db.product_variant_value.ToList();
            wishlistvm.productImageList = db.images.ToList();

            return PartialView("wishlisttable", wishlistvm);
        }

        public JsonResult addreview(string id,HomeViewModel model)
        {
            long pid = long.Parse(id);
            long uid = Convert.ToInt64(Session["user_id"]);
            review r = new review();
            r.product_id = pid;
            r.user_id = uid;
            r.review1 = model.review;
            r.star = model.star;
            db.reviews.Add(r);
            db.SaveChanges();

            product p = db.products.Where(x => x.product_id == pid).SingleOrDefault();
            List<review> reviews = db.reviews.Where(x => x.product_id == pid).ToList();
            long avg = 0;
            long Count = reviews.Count;
            foreach(review re in reviews)
            {
                avg += re.star;
            }
            avg = avg / Count;
            p.rating = avg;
            p.no_users_rated = reviews.Count;
            db.SaveChanges();
            return Json(new { result = true }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult checkout()
        {
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                CheckoutViewModel cvm = new CheckoutViewModel();
                long uid = Convert.ToInt64(Session["user_id"]);
                cvm.addressList = db.User_address.Where(x => x.user_id == uid).ToList();
                cvm.cityList = db.cities.ToList();
                cvm.countryList = db.Countries.ToList();
                cvm.addtypeList = db.address_type.ToList();
                cvm.cartList = db.carts.Where(x => x.user_id == uid).ToList();
                cvm.productVariantList = db.product_variant_value.ToList();
                cvm.productImageList = db.images.ToList();
                cvm.GrandTotal = 0;
                return View(cvm);
            }
        }

        public JsonResult checkcoupon(string coupon)
        {

            coupen c = db.coupens.Where(x => x.name.ToString().ToLower().Equals(coupon.ToLower())).SingleOrDefault();
            long uid = Convert.ToInt64(Session["user_id"]);
            List<cart> cartList= db.carts.Where(x=>x.user_id==uid).ToList();
            double GrandTotal=0;
            foreach(cart cartitem in cartList)
            {
                 product_variant_value product = db.product_variant_value.Where(x => x.product_variant_id == cartitem.product_variant_id).SingleOrDefault();
                 GrandTotal += product.Selling_price * cartitem.qty;
            }

            if (c == null)
            {
                return Json(new { isApplied = 0 , rate = 0, }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                DateTime today = System.DateTime.Now;
                if (c.starting_date.Date.CompareTo(today.Date) <= 0 && c.ending_date.Date.CompareTo(today.Date) >= 0)
                {
                    if(GrandTotal<=c.maximum_purchase_price && GrandTotal>=c.minimun_purchase_price)
                    {
                        return Json(new { isApplied = 1, rate =c.discount_rate, }, JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                         return Json(new { isApplied = 2, rate = 0, }, JsonRequestBehavior.DenyGet);
                    }
                    
                }
                else
                {
                    return Json(new { isApplied = 3, rate = 0, }, JsonRequestBehavior.DenyGet);
                }
                
            }
        }

        public JsonResult savedata(string ptotal,string sadd,string badd,string popt ,string coupon)
        {
            Session["ptotal"] = ptotal;
            Session["sadd"] = sadd;
            Session["badd"] = badd;
            Session["popt"] = popt;
            Session["coupon"] = coupon;
            return Json(new { Success=true }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult CreateOrder()
        {
            if(Session["popt"].ToString().Equals("cod"))
            {
                return RedirectToAction("Success", "Home", new { area = "User" });
            }
            else
            {
                Random randomObj = new Random();
                string transactionId = randomObj.Next(10000000, 100000000).ToString();
                

                long amount = Convert.ToInt64(Session["ptotal"]);

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_ZYr5Nup0zQgsRb", "LOfBTELam5tg9FKvBuadaVUi");
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", amount * 100);  // Amount will in paise
                options.Add("receipt", transactionId);
                options.Add("currency", "INR");
                options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                //options.Add("notes", "-- You can put any notes here --");

                Razorpay.Api.Order orderResponse = client.Order.Create(options);
                string orderId = orderResponse["id"].ToString();

                long uid=Convert.ToInt64(Session["user_id"]);
                long baddid=Convert.ToInt64(Session["badd"]);

                User_account user = db.User_account.Where(x => x.user_id == uid).SingleOrDefault();
                User_address badd=db.User_address.Where(x=>x.user_add_id==baddid).SingleOrDefault();
                city c = db.cities.Where(x => x.city_id == badd.city_id).SingleOrDefault();

                OrderViewModel orderModel = new OrderViewModel
                {
                    orderId = orderResponse.Attributes["id"],
                    razorpayKey = "rzp_test_ZYr5Nup0zQgsRb",
                    amount = amount * 100,
                    currency = "INR",
                    name = user.name,
                    email = user.email_id,
                    contactNumber = user.mobile_no,
                    address = badd.address+c.city_name,
                    description = "Description"
                };

                // Return on PaymentPage with Order data
                return View("PaymentPage", orderModel);
            }
        }

        [HttpPost]
        public ActionResult Complete()
        {
            // Payment data comes in url so we have to get it from url

            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = Request.Params["rzp_paymentid"];

            // This is orderId
            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_ZYr5Nup0zQgsRb", "LOfBTELam5tg9FKvBuadaVUi");

            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];

            //// Check payment made successfully

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                // Create these action method
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }

        public ActionResult Success()
        {
            long shippingadd = Convert.ToInt64(Session["sadd"]);
            long billingadd = Convert.ToInt64(Session["badd"]);
            long amount = Convert.ToInt64(Session["ptotal"]);
            string popt = Session["popt"].ToString();
            long uid = Convert.ToInt64(Session["user_id"]);
            long statusid=db.order_status.Where(x=>x.status=="waiting for confirm your order").SingleOrDefault().status_id;
            Order order = new Order();
            Random randomObj = new Random();
            order.order_no = randomObj.Next(10000000, 100000000).ToString();
            order.order_date = System.DateTime.Now;
            order.status_id = statusid;
            order.billing_add = billingadd;
            order.shipping_add = shippingadd;
            order.user_id = uid;
            if(popt.Equals("cod"))
            {
                long pmid=db.payment_mode.Where(x=>x.payment_mode1 == "Case On Delivery").SingleOrDefault().payment_mode_id;
                order.payment_mode_id = pmid;
            }
            else
            {
                long pmid = db.payment_mode.Where(x => x.payment_mode1 == "Online Payment").SingleOrDefault().payment_mode_id;
                order.payment_mode_id = pmid;
            }
            string coupon = Session["coupon"].ToString();
            if (coupon != "")
            {
                long coupenid = db.coupens.Where(x => x.name.ToString().ToLower().Equals(coupon.ToLower())).SingleOrDefault().coupen_id;
                order.coupen_id = coupenid;
            }
            order.shiprate = 20;
            db.Orders.Add(order);
            db.SaveChanges();

            List<cart> cartList = db.carts.Where(x => x.user_id == uid).ToList();
            foreach (cart cartitem in cartList)
            {
                product_variant_value product = db.product_variant_value.Where(x => x.product_variant_id == cartitem.product_variant_id).SingleOrDefault();
                Order_detail o = new Order_detail();
                o.currency_type_id = 1;
                o.order_id = order.order_id;
                o.product_variant_id = cartitem.product_variant_id;
                o.total_amount = product.Selling_price;
                o.quantity = cartitem.qty;
                db.Order_detail.Add(o);
                
                db.carts.Remove(cartitem);

                product.unit_in_stock = product.unit_in_stock - cartitem.qty;
                db.SaveChanges();
            }
            Session.Remove("ptotal");
            Session.Remove("sadd");
            Session.Remove("badd");
            Session.Remove("popt");
            Session.Remove("coupon");
            return View();
        }

        public ActionResult Failed()
        {
            Session.Remove("ptotal");
            Session.Remove("sadd");
            Session.Remove("badd");
            Session.Remove("popt");
            Session.Remove("coupon");
            return View();
        }

        public ActionResult orderstatus()
        {
            if (Session["user_id"] == null)
                return RedirectToAction("login", "Login", new { area = "" });
            else
            {
                OrderModel ordermodel = new OrderModel();
                long uid = Convert.ToInt64(Session["user_id"]);
                long sid = db.order_status.Where(x => x.status == "return").SingleOrDefault().status_id;
                long did = db.order_status.Where(x => x.status == "deliverd").SingleOrDefault().status_id;
                ordermodel.OrderList = db.Orders.Where(x => x.user_id == uid && x.status_id !=sid && x.status_id!=did).OrderByDescending(x => x.order_date).ToList();
                ordermodel.returnOrderList = db.Orders.Where(x => x.user_id == uid && x.status_id == sid).OrderByDescending(x => x.order_date).ToList();
                ordermodel.deliveredOrderList = db.Orders.Where(x => x.user_id == uid && x.status_id == did).OrderByDescending(x => x.order_date).ToList();
               
                ordermodel.StatusList = db.order_status.ToList();
                ordermodel.OrderDetailList = db.Order_detail.ToList();
                ordermodel.ProductVariantList = db.product_variant_value.ToList();
                ordermodel.imageList = db.images.ToList();
                List<return_reason> resonList = db.return_reason.ToList();
                ViewBag.returnReasonList = new SelectList(resonList, "return_reason_id", "return_reason1");
                return View(ordermodel);
            }
        }

        [HttpPost]
        public ActionResult createreturnorder(string id,OrderModel model)
        {
            long oid = Convert.ToInt64(id);
            Order o = db.Orders.Where(x => x.order_id == oid).SingleOrDefault();
            long sid = db.order_status.Where(x => x.status == "return").SingleOrDefault().status_id;
            o.status_id = sid;
            db.SaveChanges();
            order_return or = new order_return();
            or.order_id = oid;
            long rsid = db.return_status.Where(x => x.return_status1 == "pending").SingleOrDefault().return_status_id;
            or.return_status_id = rsid;
            or.return_reason_id = model.return_reason_id;
            db.order_return.Add(or);
            db.SaveChanges();
            return RedirectToAction("orderstatus", "Home", new { area = "User" });
        }
        
    }
}