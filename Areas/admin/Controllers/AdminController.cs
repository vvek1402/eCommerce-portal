using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce.Repository;

namespace E_Commerce.Areas.admin.Controllers
{
    public class AdminController : Controller
    {
        Entities1 db = new Entities1();
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // GET: Admin
        public ActionResult dashboard()
        {
            if (Session["user_id"] == null)
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            else
            {
                var users = db.User_account;
                foreach (User_account user in users)
                {
                    if (user.user_id == Convert.ToInt32(Session["user_id"]))
                    {
                        ViewBag.admin_name = user.name;
                        break;
                    }
                }
                return View();
            }
        }


        public ActionResult Product(string id)
        {
            if (id == null||string.IsNullOrEmpty(id))
            {
                id = "1";
            }
         
            if (Session["user_id"] == null)
                
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                ProductList list = new ProductList();
                list.productList = db.products.ToList();
                int totalRecords = 0;
                totalRecords = list.productList.Count();
                int pagesize = 5;
                int start = (int.Parse(id) - 1) * 5;
                if (start == -1)
                    start = 0;
                int end = 5;
                if (end > totalRecords - start)
                    end = totalRecords - start;
                list.productList = db.products.ToList().GetRange(start,end);
                list.categoryList = db.categories.ToList();
                list.subcategoryList = db.sub_category.ToList();
                list.productvariantList = db.product_variant_value.ToList();
                list.ImageList = db.images.ToList();

                
                long totalPage = 0;
                
                totalPage = (totalRecords / pagesize) + ((totalRecords % pagesize) > 0 ? 1 : 0);
                list.total_page = int.Parse(totalPage.ToString());
                return View(list);
            }
              
        }
        public JsonResult GetAllProduct(int id)
        {
            ProductList list = new ProductList();
            list.productList = db.products.ToList();
            int totalRecords = 0;
            totalRecords = list.productList.Count();

           
            int pagesize = 5;
            int start = (id - 1)*5;
            int end=5;
            if (end > totalRecords-start)
                end = totalRecords - start;
            
            list.productList = list.productList.GetRange(start,end);
                     
            
            long totalPage = 0;
            
            totalPage = (totalRecords / pagesize) + ((totalRecords % pagesize) > 0 ? 1 : 0);
            return Json(new { list = list.productList, pagecount = totalPage }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult simpleform()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                List<category> CategoryList = db.categories.ToList();
                ViewBag.CategoryList = new SelectList(CategoryList, "category_id", "category_name");
                List<price_decision_factor> PriceFactorList = db.price_decision_factor.ToList();
                ViewBag.PriceFactorList = new SelectList(PriceFactorList, "price_factor_id","unit");

                return View();
            }
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult simpleform(CascadingCatSubCat model, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5)
        {
            product p = new product();
            p.admin_id = Convert.ToInt64(Session["user_id"]);
            p.category_id = model.category_id;
            p.sub_category_id = model.sub_category_id;
            p.sku = model.sku;
            p.title = model.title;
            p.description = model.description;
            p.Isactive = 1;
            p.price_factor_id = model.price_factor_id;
            p.shipping_rate = model.shipping_rate;
            p.no_users_rated = 0;
            p.rating=0;
            p.isvariant = 0;
            db.products.Add(p);
            db.SaveChanges();
            
            product_variant_value variant = new product_variant_value();
            variant.product_id = p.product_id;
            variant.variant_value_id = null;
            variant.MRP = model.MRP;
            variant.Selling_price = model.Selling_price;
            variant.discount = model.discount;
            variant.discount_max_quantity = model.discount_max_quantity;
            variant.discount_min_quantity = model.discount_min_quantity;
            variant.weight = model.weight;
            variant.unit_in_stock = model.unit_in_stock;
            variant.max_purchase_quantity = model.max_purchase_quantity;
            variant.quantity_per_unit = model.quantity_per_unit;
            variant.sku = model.sku;
            variant.product_variant_title = model.title;
            db.product_variant_value.Add(variant);
            db.SaveChanges();

            image img = new image();
            img.product_variant_id = variant.product_variant_id;
            if (file1 != null)
            {
                string extension = System.IO.Path.GetExtension(file1.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file1.SaveAs(path);
                img.main_image = "/../../../Includes/product-images/" + newFileName;
            }
            if (file2 != null)
            {
                string extension = System.IO.Path.GetExtension(file2.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file2.SaveAs(path);
                img.image1 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file3 != null)
            {
                string extension = System.IO.Path.GetExtension(file3.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file3.SaveAs(path);
                img.image2 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file4 != null)
            {
                string extension = System.IO.Path.GetExtension(file4.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file4.SaveAs(path);
                img.image3 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file5 != null)
            {
                string extension = System.IO.Path.GetExtension(file5.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file5.SaveAs(path);
                img.image4 = "/../../../Includes/product-images/" + newFileName;
            }

            db.images.Add(img);

            db.SaveChanges();
            
            return RedirectToAction("product", "Admin", new { area = "admin" }); 
        }

        [HttpGet]
        public ActionResult editsimpleproduct(string id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                long pid = Convert.ToInt64(id);
                List<category> CategoryList = db.categories.ToList();
                ViewBag.CategoryList = new SelectList(CategoryList, "category_id", "category_name");
                List<sub_category> SubCategoryList = db.sub_category.ToList();
                ViewBag.SubCategoryList = new SelectList(SubCategoryList, "sub_category_id", "sub_category_name");
                List<price_decision_factor> PriceFactorList = db.price_decision_factor.ToList();
                ViewBag.PriceFactorList = new SelectList(PriceFactorList, "price_factor_id", "unit");

                CascadingCatSubCat model = new CascadingCatSubCat();
                model.product = db.products.Where(x => x.product_id == pid).SingleOrDefault();
                product_variant_value pvv= db.product_variant_value.Where(x=>x.product_id==pid).SingleOrDefault();
                image i = db.images.Where(x => x.product_variant_id == pvv.product_variant_id).SingleOrDefault();

                model.cat = db.categories.Where(x => x.category_id == model.product.category_id).SingleOrDefault();
                model.subcat = db.sub_category.Where(x => x.sub_category_id == model.product.sub_category_id).SingleOrDefault();
                model.sku=model.product.sku;
                model.title = model.product.title;
                model.unit_in_stock = pvv.unit_in_stock;
                model.quantity_per_unit = pvv.quantity_per_unit;
                model.max_purchase_quantity = pvv.max_purchase_quantity;
                model.weight = pvv.weight;
                model.description = model.product.description;
                model.MRP = pvv.MRP;
                model.Selling_price = pvv.Selling_price;
                model.discount = pvv.discount;
                model.discount_min_quantity = pvv.discount_min_quantity;
                model.discount_max_quantity = pvv.discount_max_quantity;
                model.shipping_rate = model.product.shipping_rate;
                model.main_image = i.main_image;
                model.image1 = i.image1;
                model.image2 = i.image2;
                model.image3 = i.image3;
                model.image4 = i.image4;
                return View(model);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editsimpleproduct(string id,CascadingCatSubCat model, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5)
        {
            long pid = Convert.ToInt64(id);
            product p = db.products.Where(x=>x.product_id==pid).SingleOrDefault();
            p.admin_id = Convert.ToInt64(Session["user_id"]);
            p.category_id = model.category_id;
            p.sub_category_id = model.sub_category_id;
            p.sku = model.sku;
            p.title = model.title;
            p.description = model.description;
            p.Isactive = 1;
            p.price_factor_id = model.price_factor_id;
            p.shipping_rate = model.shipping_rate;
            p.no_users_rated = 0;
            p.rating = 0;
            p.isvariant = 0;
            db.SaveChanges();

            product_variant_value variant = db.product_variant_value.Where(x => x.product_id == pid).SingleOrDefault();
            variant.product_id = p.product_id;
            variant.variant_value_id = null;
            variant.MRP = model.MRP;
            variant.Selling_price = model.Selling_price;
            variant.discount = model.discount;
            variant.discount_max_quantity = model.discount_max_quantity;
            variant.discount_min_quantity = model.discount_min_quantity;
            variant.weight = model.weight;
            variant.unit_in_stock = model.unit_in_stock;
            variant.max_purchase_quantity = model.max_purchase_quantity;
            variant.quantity_per_unit = model.quantity_per_unit;
            variant.sku = model.sku;
            variant.product_variant_title = model.title;
            db.SaveChanges();

            image img = db.images.Where(x => x.product_variant_id == variant.product_variant_id).SingleOrDefault();
            img.product_variant_id = variant.product_variant_id;
            if (file1 != null)
            {
                string extension = System.IO.Path.GetExtension(file1.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file1.SaveAs(path);
                img.main_image = "/../../../Includes/product-images/" + newFileName;
            }
            if (file2 != null)
            {
                string extension = System.IO.Path.GetExtension(file2.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file2.SaveAs(path);
                img.image1 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file3 != null)
            {
                string extension = System.IO.Path.GetExtension(file3.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file3.SaveAs(path);
                img.image2 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file4 != null)
            {
                string extension = System.IO.Path.GetExtension(file4.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file4.SaveAs(path);
                img.image3 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file5 != null)
            {
                string extension = System.IO.Path.GetExtension(file5.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file5.SaveAs(path);
                img.image4 = "/../../../Includes/product-images/" + newFileName;
            }

            db.SaveChanges();

            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());

        }

     
      
        public JsonResult GetSubCategoryList(int category_id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<sub_category> SubCategoryList = db.sub_category.Where(x => x.category_id == category_id).ToList();
            return Json(SubCategoryList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult userList(int? pageNumber)
        {
            if (Session["user_id"] == null)
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            else
            {
                //List<User_account> users = _unitOfWork.GetRepositoryInstance<User_account>().GetAllRecordsIQueryable().Where(x => x.role_id == 2).ToList();
                
                UserDetails user = new UserDetails();
                user.UserList = db.User_account.ToList();
                user.addresslist = db.User_address.ToList();
                user.cityList = db.cities.ToList();
                user.countryList = db.Countries.ToList();
                user.addtypeList = db.address_type.ToList();
                return View(user);
            }
        }

         [HttpGet]
        public ActionResult edituser(long id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                UserDetails user = new UserDetails();
                user.user_account = db.User_account.Where(x => x.user_id == id).SingleOrDefault();
                user.addresslist = db.User_address.Where(x => x.user_id == id).ToList();
                user.cityList = db.cities.ToList();
                ViewBag.cityList = new SelectList(user.cityList, "city_id", "city_name");
                user.countryList = db.Countries.ToList();
                ViewBag.countryList = new SelectList(user.countryList, "country_id", "country_name");
                user.addtypeList = db.address_type.ToList();
                ViewBag.addtypeList = new SelectList(user.addtypeList, "address_type_id", "address_type1");
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult edituser(UserDetails model,long id)
         {
             int flag = 0;
             
             User_account user = db.User_account.Where(x => x.user_id == id).SingleOrDefault();
             if (user.name != model.name)
                 user.name = model.name;
             if (user.email_id != model.email_id)
             {

                 User_account obj = db.User_account.Where(x => x.email_id == model.email_id).SingleOrDefault();
                 if (obj != null)
                 {
                     ViewBag.error = "*Email id already have account";
                     flag = 1;
                 }
                 else
                 {
                     user.email_id = model.email_id;
                 }
             }
             if (user.mobile_no != model.mobile_no)
             {

                 User_account obj1 = db.User_account.Where(x => x.mobile_no == model.mobile_no).SingleOrDefault();
                 if (obj1 != null)
                 {
                     ViewBag.error1 = "*mobile no. already have account";
                     flag = 1;
                 }
                 else
                 {
                     user.mobile_no = model.mobile_no;
                 }
             }
             if (flag == 0)
                 db.SaveChanges();


             return RedirectToAction("userList", "Admin", new { area="admin"});
         }
        [HttpGet]
        public ActionResult variantform()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                List<category> CategoryList = db.categories.ToList();
                ViewBag.CategoryList = new SelectList(CategoryList, "category_id", "category_name");
                List<price_decision_factor> PriceFactorList = db.price_decision_factor.ToList();
                ViewBag.PriceFactorList = new SelectList(PriceFactorList, "price_factor_id", "unit");
                
                return View();
            }
        }


        [HttpGet]
        public ActionResult editvariantform(string id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                long pid = Convert.ToInt64(id);
                List<category> CategoryList = db.categories.ToList();
                ViewBag.CategoryList = new SelectList(CategoryList, "category_id", "category_name");
                List<sub_category> SubCategoryList = db.sub_category.ToList();
                ViewBag.SubCategoryList = new SelectList(SubCategoryList, "sub_category_id", "sub_category_name");
                List<price_decision_factor> PriceFactorList = db.price_decision_factor.ToList();
                ViewBag.PriceFactorList = new SelectList(PriceFactorList, "price_factor_id", "unit");

                CascadingCatSubCat model = new CascadingCatSubCat();
                model.product = db.products.Where(x => x.product_id == pid).SingleOrDefault();
                model.cat = db.categories.Where(x => x.category_id == model.product.category_id).SingleOrDefault();
                model.subcat = db.sub_category.Where(x => x.sub_category_id == model.product.sub_category_id).SingleOrDefault();
                
                model.cat = db.categories.Where(x => x.category_id == model.product.category_id).SingleOrDefault();
                model.subcat = db.sub_category.Where(x => x.sub_category_id == model.product.sub_category_id).SingleOrDefault();
                model.sku = model.product.sku;
                model.title = model.product.title;
                model.description = model.product.description;
                model.shipping_rate = model.product.shipping_rate;

               
                model.productvariantList = db.product_variant_value.Where(x => x.product_id == pid).ToList();
                model.variantvalueList = db.variant_value.ToList();
                model.variantList = db.variants.ToList();
                model.ImageList = db.images.ToList();

                return View(model);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult variantform(CascadingCatSubCat model)
        {
            product p = new product();
            p.admin_id = Convert.ToInt32(Session["user_id"]);
            p.category_id = model.category_id;
            p.sub_category_id = model.sub_category_id;
            p.sku = model.sku;
            p.title = model.title;
            p.description = model.description;
            p.Isactive = 1;
            p.price_factor_id = model.price_factor_id;
            p.shipping_rate = model.shipping_rate;
            p.no_users_rated = 0;
            p.isvariant = 1;
            db.products.Add(p);
            db.SaveChanges();

            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
 
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editvariantform(string id,CascadingCatSubCat model)
        {
            long pid = Convert.ToInt64(id);
            product p = db.products.Where(x => x.product_id == pid).SingleOrDefault();
            p.admin_id = Convert.ToInt32(Session["user_id"]);
            p.category_id = model.category_id;
            p.sub_category_id = model.sub_category_id;
            p.sku = model.sku;
            p.title = model.title;
            p.description = model.description;
            p.Isactive = 1;
            p.price_factor_id = model.price_factor_id;
            p.shipping_rate = model.shipping_rate;
            p.no_users_rated = 0;
            p.isvariant = 1;
            db.SaveChanges();

            return RedirectToAction("product", "Admin", new { area = "admin" });
        }

        [HttpGet]
        public ActionResult addvariant(int id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                ProductList list = new ProductList();
               list.productvariantList= db.product_variant_value.Where(x => x.product_id==id).ToList();
               list.variantvalueList = db.variant_value.ToList();
               list.variantList = db.variants.ToList();
               list.ImageList = db.images.ToList();

                product Product = db.products.Where(x=>x.isvariant==1 && x.product_id == id).FirstOrDefault();
                ViewBag.ProductTitle = Product.title;
                List<variant> VariantList = db.variants.Where(x=>x.sub_category_id==Product.sub_category_id).ToList();
                ViewBag.VariantList = new SelectList(VariantList, "variant_id", "variant_name");
                               
                return View(list);
            }
        }

        [HttpGet]
        public ActionResult editvariant(string id)
        {
            long pvid = Convert.ToInt64(id);
            CascadingCatSubCat list = new CascadingCatSubCat();
            product_variant_value pv = db.product_variant_value.Where(x => x.product_variant_id == pvid).SingleOrDefault();
            list.variantsku = pv.sku;
            list.product_variant_title = pv.product_variant_title;
            
            variant_value vv = db.variant_value.Where(x => x.variant_value_id == pv.variant_value_id).SingleOrDefault();
            list.variant_value = vv.value;

            variant v = db.variants.Where(x => x.variant_id == vv.variant_id).SingleOrDefault();
            list.variant_id = v.variant_id;

            list.unit_in_stock = pv.unit_in_stock;
            list.quantity_per_unit = pv.quantity_per_unit;
            list.max_purchase_quantity = pv.max_purchase_quantity;
            list.weight = pv.weight;
            list.MRP = pv.MRP;
            list.Selling_price = pv.Selling_price;
            list.discount = pv.discount;
            list.discount_max_quantity = pv.discount_max_quantity;
            list.discount_min_quantity = pv.discount_min_quantity;

            image i = db.images.Where(x => x.product_variant_id == pvid).SingleOrDefault();
            list.main_image = i.main_image;
            list.image1 = i.image1;
            list.image2 = i.image2;
            list.image3 = i.image3;
            list.image4 = i.image4;


            long pid=pv.product_id;
            product Product = db.products.Where(x => x.isvariant == 1 && x.product_id ==pid ).SingleOrDefault();
            List<variant> VariantList = db.variants.Where(x => x.sub_category_id == Product.sub_category_id).ToList();
            ViewBag.VariantList = new SelectList(VariantList, "variant_id", "variant_name");

            return View(list);
        }

        [HttpPost]
        public ActionResult addvariant(int id, ProductList model, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5)
        {
            variant_value vvalue = new variant_value();
            vvalue.variant_id = model.variant_id;
            vvalue.value = model.variant_value;
            vvalue.description = model.variant_value;
            db.variant_value.Add(vvalue);
            db.SaveChanges();

            product_variant_value variant = new product_variant_value();
            variant.product_id = id;
            variant.product_variant_title = model.product_variant_title;
            variant.variant_value_id = vvalue.variant_value_id;
            variant.MRP = model.MRP;
            variant.Selling_price = model.Selling_price;
            variant.discount = model.discount;
            variant.discount_max_quantity = model.discount_max_quantity;
            variant.discount_min_quantity = model.discount_min_quantity;
            variant.weight = model.weight;
            variant.unit_in_stock = model.unit_in_stock;
            variant.max_purchase_quantity = model.max_purchase_quantity;
            variant.quantity_per_unit = model.quantity_per_unit;
            variant.sku = model.variantsku;
            db.product_variant_value.Add(variant);
            db.SaveChanges();
            
            image img = new image();
            img.product_variant_id = variant.product_variant_id;
            if (file1 != null)
            {
                string extension = System.IO.Path.GetExtension(file1.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension( System.IO.Path.GetRandomFileName() )+ extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file1.SaveAs(path);
                img.main_image = "/../../../Includes/product-images/"+newFileName;
            }
            if (file2 != null)
            {
                string extension = System.IO.Path.GetExtension(file2.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file2.SaveAs(path);
                img.image1 = "/../../../Includes/product-images/" + newFileName;
            } 
            if (file3 != null)
            {
                string extension = System.IO.Path.GetExtension(file3.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file3.SaveAs(path);
                img.image2 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file4 != null)
            {
                string extension = System.IO.Path.GetExtension(file4.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file4.SaveAs(path);
                img.image3 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file5 != null)
            {
                string extension = System.IO.Path.GetExtension(file5.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file5.SaveAs(path);
                img.image4 = "/../../../Includes/product-images/" + newFileName;
            }  
        
            db.images.Add(img);

            db.SaveChanges();

            return RedirectToAction("product", "Admin", new { area = "admin" });
        }

        [HttpPost]

        public ActionResult editvariant(string id, ProductList model, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5)
        {
            long pvid = Convert.ToInt64(id);
            product_variant_value variant =db.product_variant_value.Where(x=>x.product_variant_id==pvid).SingleOrDefault();
            variant_value vvalue =db.variant_value.Where(x=>x.variant_value_id==variant.variant_value_id).SingleOrDefault();
            vvalue.variant_id = model.variant_id;
            vvalue.value = model.variant_value;
            vvalue.description = model.variant_value;
            db.SaveChanges();

            
            variant.product_variant_title = model.product_variant_title;
            variant.variant_value_id = vvalue.variant_value_id;
            variant.MRP = model.MRP;
            variant.Selling_price = model.Selling_price;
            variant.discount = model.discount;
            variant.discount_max_quantity = model.discount_max_quantity;
            variant.discount_min_quantity = model.discount_min_quantity;
            variant.weight = model.weight;
            variant.unit_in_stock = model.unit_in_stock;
            variant.max_purchase_quantity = model.max_purchase_quantity;
            variant.quantity_per_unit = model.quantity_per_unit;
            variant.sku = model.variantsku;
            db.SaveChanges();

            image img =db.images.Where(x=>x.product_variant_id==pvid).SingleOrDefault();
            img.product_variant_id = variant.product_variant_id;
            if (file1 != null)
            {
                string extension = System.IO.Path.GetExtension(file1.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file1.SaveAs(path);
                img.main_image = "/../../../Includes/product-images/" + newFileName;
            }
            if (file2 != null)
            {
                string extension = System.IO.Path.GetExtension(file2.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file2.SaveAs(path);
                img.image1 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file3 != null)
            {
                string extension = System.IO.Path.GetExtension(file1.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file3.SaveAs(path);
                img.image2 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file4 != null)
            {
                string extension = System.IO.Path.GetExtension(file1.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file4.SaveAs(path);
                img.image3 = "/../../../Includes/product-images/" + newFileName;
            }
            if (file5 != null)
            {
                string extension = System.IO.Path.GetExtension(file1.FileName);
                string newFileName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName()) + extension;
                string path = System.IO.Path.Combine(Server.MapPath("~/Includes/product-images"), newFileName);
                file5.SaveAs(path);
                img.image4 = "/../../../Includes/product-images/" + newFileName;
            }


            db.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            //return RedirectToAction("product", "Admin", new { area = "admin" });
        }
        
        public ActionResult removeuser(int id)
        {
            List<User_address> address = db.User_address.Where(x=>x.user_id==id).ToList();
            foreach (User_address add in address)
            {
                db.User_address.Remove(add);
            }
            db.SaveChanges();
            User_account user = db.User_account.Where(x => x.user_id == id ).SingleOrDefault();
            db.User_account.Remove(user);
            db.SaveChanges();
            return RedirectToAction("userList", "Admin", new { area = "admin" });
        }

        
        public ActionResult SimpleProduct()
        {
            if (Session["user_id"] == null)
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            else
            {
                List<category> CategoryList = db.categories.ToList();
                ViewBag.CategoryList = new SelectList(CategoryList, "category_id", "category_name");
                List<price_decision_factor> PriceFactorList = db.price_decision_factor.ToList();
                ViewBag.PriceFactorList = new SelectList(PriceFactorList, "price_factor_id", "unit");

                ProductList list = new ProductList();
                list.productList = db.products.Where(x=>x.isvariant==0).ToList();
                list.categoryList = db.categories.ToList();
                list.subcategoryList = db.sub_category.ToList();
                list.productvariantList = db.product_variant_value.ToList();
                list.ImageList = db.images.ToList();
                return View(list);
            }
        }

        public ActionResult VariantProduct()
        {
            if (Session["user_id"] == null)
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            else
            {
                ProductList list = new ProductList();
                list.productList = db.products.Where(x => x.isvariant == 1).ToList();
                list.categoryList = db.categories.ToList();
                list.subcategoryList = db.sub_category.ToList();
                list.productvariantList = db.product_variant_value.ToList();
                list.ImageList = db.images.ToList();
                return View(list);
            }
        }

        public ActionResult manageOrder()
        {
            OrderModel ordermodel = new OrderModel();
            long order_status_id = db.order_status.Where(x => x.status == "accepted").SingleOrDefault().status_id;
            ordermodel.OrderList = db.Orders.Where(x => x.status_id == order_status_id).OrderByDescending(x=>x.order_date).ToList();
            ordermodel.StatusList = db.order_status.ToList();
            ordermodel.OrderDetailList = db.Order_detail.ToList();
            ordermodel.ProductVariantList = db.product_variant_value.ToList();
            ordermodel.paymentmodeList = db.payment_mode.ToList();
            ViewBag.StatusList = new SelectList(ordermodel.StatusList, "status_id", "status");
            return View(ordermodel);
        }

        public ActionResult pandingOrder()
        {
            OrderModel ordermodel = new OrderModel();
            long order_status_id = db.order_status.Where(x => x.status == "waiting for confirm your order").SingleOrDefault().status_id;
            ordermodel.OrderList = db.Orders.Where(x => x.status_id == order_status_id).OrderByDescending(x => x.order_date).ToList();
            ordermodel.StatusList = db.order_status.ToList();
            ordermodel.OrderDetailList = db.Order_detail.ToList();
            ordermodel.ProductVariantList = db.product_variant_value.ToList();
            ordermodel.paymentmodeList = db.payment_mode.ToList();           
            ViewBag.StatusList = new SelectList(ordermodel.StatusList, "status_id", "status");
            return View(ordermodel);
        }

        public ActionResult outfordeliverOrder()
        {
            OrderModel ordermodel = new OrderModel();
            long order_status_id = db.order_status.Where(x => x.status == "out for delivery").SingleOrDefault().status_id;
            ordermodel.OrderList = db.Orders.Where(x => x.status_id == order_status_id).OrderByDescending(x => x.order_date).ToList();
            ordermodel.StatusList = db.order_status.ToList();
            ordermodel.OrderDetailList = db.Order_detail.ToList();
            ordermodel.ProductVariantList = db.product_variant_value.ToList();
            ordermodel.paymentmodeList = db.payment_mode.ToList();
            ViewBag.StatusList = new SelectList(ordermodel.StatusList, "status_id", "status");
            return View(ordermodel);
        }
        public ActionResult rejectedOrder()
        {
            OrderModel ordermodel = new OrderModel();
            long order_status_id = db.order_status.Where(x => x.status == "rejected").SingleOrDefault().status_id;
            ordermodel.OrderList = db.Orders.Where(x => x.status_id == order_status_id).OrderByDescending(x => x.order_date).ToList();
            ordermodel.StatusList = db.order_status.ToList();
            ordermodel.OrderDetailList = db.Order_detail.ToList();
            ordermodel.ProductVariantList = db.product_variant_value.ToList();
            ordermodel.paymentmodeList = db.payment_mode.ToList();
            ViewBag.StatusList = new SelectList(ordermodel.StatusList, "status_id", "status");
            return View(ordermodel);
        }

        public ActionResult deliveredorder()
        {
            OrderModel ordermodel = new OrderModel();
            long order_status_id = db.order_status.Where(x => x.status == "deliverd").SingleOrDefault().status_id;
            ordermodel.OrderList = db.Orders.Where(x => x.status_id == order_status_id).OrderByDescending(x => x.order_date).ToList();
            ordermodel.StatusList = db.order_status.ToList();
            ordermodel.OrderDetailList = db.Order_detail.ToList();
            ordermodel.ProductVariantList = db.product_variant_value.ToList();
            ordermodel.paymentmodeList = db.payment_mode.ToList();
            ViewBag.StatusList = new SelectList(ordermodel.StatusList, "status_id", "status");
            return View(ordermodel);
        }

        [HttpPost]
        public ActionResult acceptorder(string id,OrderModel model)
        {
            long order_id = Convert.ToInt64(id);
            Order order = db.Orders.Where(x => x.order_id == order_id).SingleOrDefault();
            long sid = db.order_status.Where(x => x.status == "accepted").SingleOrDefault().status_id;            
            order.status_id = sid;
            order.shipdate = model.shipdate;
            db.SaveChanges();
            return RedirectToAction("pandingOrder", "admin", new { area = "admin" });
        }

        
        public ActionResult rejectorder(string id)
        {
            long order_id = Convert.ToInt64(id);
            Order order = db.Orders.Where(x => x.order_id == order_id).SingleOrDefault();
            long sid = db.order_status.Where(x => x.status == "rejected").SingleOrDefault().status_id;
            order.status_id = sid;
            db.SaveChanges();
            return RedirectToAction("pandingOrder", "admin", new { area = "admin" });
        }

        [HttpPost]
        public ActionResult editstatus(int id,OrderModel model)
        {
            long order_id=Convert.ToInt64(id);
            Order order = db.Orders.Where(x => x.order_id==order_id).SingleOrDefault();
            order.status_id = model.status_id;
            db.SaveChanges();
            return RedirectToAction("manageOrder", "Admin", new { area = "admin" });
        }


        [HttpGet]
        public ActionResult cancelorder(int id)
        {
            long order_id = Convert.ToInt64(id);
            List<Order_detail> order_details = db.Order_detail.Where(x => x.order_id == order_id).ToList();
            foreach (Order_detail o in order_details)
                db.Order_detail.Remove(o);
                
            Order order = db.Orders.Where(x => x.order_id == order_id).SingleOrDefault();

            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("manageOrder", "Admin", new { area = "admin" });
        }

        [HttpGet]
        public ActionResult printinvoice(int id)
        {
            OrderModel ordermodel = new OrderModel();
            long order_id = Convert.ToInt64(id);
            long admin_id = Convert.ToInt64(Session["user_id"]);
            User_account admin=db.User_account.Where(x=>x.user_id==admin_id).SingleOrDefault();
            ordermodel.users = db.User_account.ToList();
            ordermodel.order = db.Orders.Where(x => x.order_id == order_id).SingleOrDefault();
            ordermodel.cities = db.cities.ToList();
            ordermodel.countries = db.Countries.ToList();
            ordermodel.OrderDetailList = db.Order_detail.Where(x => x.order_id == order_id).ToList();
            ordermodel.badd = db.User_address.Where(x => x.user_add_id == ordermodel.order.billing_add).SingleOrDefault();
            ordermodel.sadd = db.User_address.Where(x => x.user_add_id == ordermodel.order.shipping_add).SingleOrDefault();
            ordermodel.adminadd = db.User_address.Where(x => x.user_id == admin.user_id && x.isdefault==1).SingleOrDefault();
            ordermodel.ProductVariantList = db.product_variant_value.ToList();

            return View(ordermodel);
        }

        public ActionResult addcategory()
        {
            
            ProductList list = new ProductList();

            list.categoryList = db.categories.ToList();
            list.subcategoryList = db.sub_category.ToList();

            return View(list);  
        }
        public ActionResult getcategory()
        {

            ProductList list = new ProductList();

            list.categoryList = db.categories.ToList();
            list.subcategoryList = db.sub_category.ToList();

            return PartialView("partialcategory",list);
        }

        public JsonResult changestatus(string id)
        {
            long cid = Convert.ToInt64(id);
            category c = db.categories.Where(x => x.category_id == cid).SingleOrDefault();
            if (c.Isactive == 1)
                c.Isactive = 0;
            else
                c.Isactive = 1;
            db.SaveChanges();
            return Json(new { Success = true }, JsonRequestBehavior.DenyGet);

        }

        public ActionResult getsubcategory(string id)
        {
            long cid = int.Parse(id);
            List<sub_category> subcategoryList = db.sub_category.Where(x=>x.category_id==cid).ToList();

            return Json(new { data = subcategoryList }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult addcategory(ProductList model)
        {
           category add = new category();
            add.category_name = model.category_name;
            add.Isactive = 1;
            add.description = "categories";


            db.categories.Add(add);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { IsValid = true, Success = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return RedirectToAction("addcategory", "Admin", new { area = "admin" });
            }
            
        }

        [HttpPost]
        public ActionResult addsubcategory(int id ,ProductList model)
        {
            sub_category add = new sub_category();
            add.sub_category_name = model.sub_category_name;
            add.Isactive = 1;
            add.description = "sub categories";
            add.category_id = Convert.ToInt64(id);

            db.sub_category.Add(add);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { IsValid = true, Success = true }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return RedirectToAction("addcategory", "Admin", new { area = "admin" });
            }
           
        }

        public ActionResult coupen()
        {
            CoupenViewModel list = new CoupenViewModel();


            list.coupenList = db.coupens.ToList();

            return View(list);

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult coupen(CoupenViewModel model)
        {
            coupen add = new coupen();
            add.name = model.name;

            add.description = model.description;
            add.starting_date = model.starting_date;
            add.ending_date = model.ending_date;

            add.discount_rate = model.discount_rate;
            add.minimun_purchase_price = model.minimun_purchase_price;
            add.maximum_purchase_price = model.maximum_purchase_price;

          



            db.coupens.Add(add);
            db.SaveChanges();

            return RedirectToAction("coupen", "Admin", new { area = "admin" });
        }

        public JsonResult removeproduct(string id)
        {
            long pid = Convert.ToInt64(id);
            product p = db.products.Where(x => x.product_id == pid).SingleOrDefault();
            if(p.isvariant==1)
            {
                List<product_variant_value> pvList = db.product_variant_value.Where(x => x.product_id == pid).ToList();
                foreach(product_variant_value pv in pvList)
                {
                    image i = db.images.Where(x => x.product_variant_id == pv.product_variant_id).SingleOrDefault();
                    variant_value vv = db.variant_value.Where(x => x.variant_value_id == pv.variant_value_id).SingleOrDefault();
                    db.images.Remove(i);
                    db.variant_value.Remove(vv);
                    db.product_variant_value.Remove(pv);
                }
                db.products.Remove(p);
            }
            else
            {
                product_variant_value pv = db.product_variant_value.Where(x => x.product_id == pid).SingleOrDefault();
                image i = db.images.Where(x => x.product_variant_id == pv.product_variant_id).SingleOrDefault();
                db.images.Remove(i);
                db.product_variant_value.Remove(pv);
                db.products.Remove(p);
            }
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult gettotalearning()
        {
            double earning =0;
            long sales = 0;

            long wid=db.order_status.Where(x=>x.status=="waiting for confirm your order").SingleOrDefault().status_id;
            long rid=db.order_status.Where(x=>x.status=="rejected").SingleOrDefault().status_id;
            List<Order> orders = db.Orders.Where(x => x.status_id != wid && x.status_id != rid).ToList();

            foreach(Order order in orders)
            {
                double ordertotal=20;
                long oid = order.order_id;
                List<Order_detail> orderdetails = db.Order_detail.Where(x => x.order_id == oid).ToList();
                foreach(Order_detail orderdetail in orderdetails)
                {
                    ordertotal += orderdetail.total_amount*orderdetail.quantity;
                    sales++;
                }
                double discount;
                if (order.coupen_id!=null)
                {

                    discount = db.coupens.Where(x => x.coupen_id == order.coupen_id).SingleOrDefault().discount_rate;
                    ordertotal -= discount;

                }
                earning += ordertotal;
            }

            return Json(new { earning=earning,sales=sales},JsonRequestBehavior.DenyGet);
        }


        public JsonResult getunshippedorders()
        {


            long aid = db.order_status.Where(x => x.status == "accepted").SingleOrDefault().status_id;
           List<Order> orders = db.Orders.Where(x => x.status_id == aid).ToList();
           long count = orders.Count;
            

            return Json(new { count =  count }, JsonRequestBehavior.DenyGet);
        }

        public JsonResult totaluser()
        {
           List<User_account> users = db.User_account.ToList();
           long count = users.Count;
            
           return Json(new { count =  count }, JsonRequestBehavior.DenyGet);
        }

        public JsonResult monthlyearning()
        {
            double Jan = 0;
            double Feb = 0;
            double Mar = 0;
            double Apr = 0;
            double May = 0;
            double Jun = 0;
            double Jul = 0;
            double Aug = 0;
            double Sep = 0;
            double Oct = 0;
            double Nov = 0;
            double Dec = 0;

            long wid=db.order_status.Where(x=>x.status=="waiting for confirm your order").SingleOrDefault().status_id;
            long rid=db.order_status.Where(x=>x.status=="rejected").SingleOrDefault().status_id;
            List<Order> orders = db.Orders.Where(x => x.status_id != wid && x.status_id != rid).ToList();

            foreach(Order order in orders)
            {
                double ordertotal = 20;
                long oid = order.order_id;
                List<Order_detail> orderdetails = db.Order_detail.Where(x => x.order_id == oid).ToList();
                foreach (Order_detail orderdetail in orderdetails)
                {
                    ordertotal += orderdetail.total_amount * orderdetail.quantity;
                   
                }
                double discount;
                if (order.coupen_id != null)
                {

                    discount = db.coupens.Where(x => x.coupen_id == order.coupen_id).SingleOrDefault().discount_rate;
                    ordertotal -= discount;

                }

                if      (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 1)   Jan += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 2)   Feb += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 3)   Mar += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 4)   Apr += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 5)   May += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 6)   Jun += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 7)   Jul += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 8)   Aug += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 9)   Sep += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 10)  Oct += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 11)  Nov += ordertotal;
                else if (order.order_date.Year==System.DateTime.Now.Year && order.order_date.Month == 12)  Dec += ordertotal;
            }

            return Json(new { 
            Jan=Jan,
            Feb=Feb,
            Mar=Mar,
            Apr=Apr,
            May=May,
            Jun=Jun,
            Jul=Jul,
            Aug=Aug,
            Sep=Sep,
            Oct=Oct,
            Nov=Nov,
            Dec=Dec,
            },JsonRequestBehavior.DenyGet);
        }

        public JsonResult countsoldcatagory()
        {
            List<category> cList=db.categories.ToList();
            List<string> catList = cList.Select(x => x.category_name).ToList();
            List<long> cidList = cList.Select(x => x.category_id).ToList();
            List<long> countList = new List<long>(cList.Count);

            long wid = db.order_status.Where(x => x.status == "waiting for confirm your order").SingleOrDefault().status_id;
            long rid = db.order_status.Where(x => x.status == "rejected").SingleOrDefault().status_id;
            List<Order> orders = db.Orders.Where(x => x.status_id != wid && x.status_id != rid).ToList();
            List<product> pList=new List<product>();
            foreach (Order order in orders)
            {
                long oid = order.order_id;
                List<Order_detail> orderdetails = db.Order_detail.Where(x => x.order_id == oid).ToList();
                
                foreach (Order_detail orderdetail in orderdetails)
                {
                    product_variant_value pv = db.product_variant_value.Where(x=>x.product_variant_id==orderdetail.product_variant_id).SingleOrDefault();
                    pList.Add(db.products.Where(x => x.product_id == pv.product_id).SingleOrDefault());
                }
            }

            foreach(long cid in cidList)
            {
                long count = pList.Where(x => x.category_id == cid).ToList().Count;
                countList.Add(count);
            }


            return Json(new { catList=catList,countList=countList}, JsonRequestBehavior.DenyGet);
        }

        public JsonResult getreview()
        {
            List<string> review = new List<string>();
            List<string> uname = new List<string>();
            List<string> pname = new List<string>();
            List<long> pvid = new List<long>();
            List<review> reviews = db.reviews.OrderByDescending(x=>x.review_id).ToList();
            if(reviews.Count>6)
            {
                int i = 0;
                foreach(review r in reviews)
                {
                    uname.Add(db.User_account.Where(x => x.user_id == r.user_id).SingleOrDefault().name);
                    product p = db.products.Where(x => x.product_id == r.product_id).SingleOrDefault();
                    pname.Add(p.title);
                    pvid.Add(p.product_id);
                    review.Add(r.review1);
                    i++;
                    if (i > 6)
                        break;
                }
            }
            else
            {
                foreach (review r in reviews)
                {
                    uname.Add(db.User_account.Where(x => x.user_id == r.user_id).SingleOrDefault().name);
                    product p = db.products.Where(x => x.product_id == r.product_id).SingleOrDefault();
                    pname.Add(p.title);
                    pvid.Add(p.product_id);
                    review.Add(r.review1);
                }
            }
            
            return Json(new { 
                review=review,
                pname=pname,
                uname=uname,
                pvid=pvid,

            },JsonRequestBehavior.DenyGet);
        }

        public JsonResult recentorders()
        {
            List<RecentOrder> data = new List<RecentOrder>();
            long order_status_id = db.order_status.Where(x => x.status == "waiting for confirm your order").SingleOrDefault().status_id;
            
            List<Order> olist = db.Orders.Where(x=>x.status_id==order_status_id).OrderByDescending(x => x.order_date).Take(5).ToList();
            
            foreach(Order o  in olist)
            {
                RecentOrder rolist = new RecentOrder();
                List<Order_detail> odlist = db.Order_detail.Where(x => x.order_id == o.order_id).ToList();
                rolist.order_no = o.order_no;
                List<string> plist=new List<string>();
                foreach(Order_detail od in odlist)
                {
                    product_variant_value pv= db.product_variant_value.Where(x=>x.product_variant_id==od.product_variant_id).SingleOrDefault();
                    plist.Add(pv.product_variant_title);
                    
                }
                rolist.productlist = plist;
                rolist.customername = db.User_account.Where(x => x.user_id == o.user_id).SingleOrDefault().name;
                rolist.orderdate = o.order_date.Date.ToString("dd/MM/yyyy");
                rolist.paymentmode = db.payment_mode.Where(x => x.payment_mode_id == o.payment_mode_id).SingleOrDefault().payment_mode1;
                data.Add(rolist);
            }

            return Json(new
            {
              data=data, 
            }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult reviews()
        {
            ReviewViewModel model = new ReviewViewModel();
            model.reviewList = db.reviews.ToList();
            model.productList = db.products.ToList();
            model.userList = db.User_account.ToList();
            return View(model);
        }

        public ActionResult returnpanding()
        {
            OrderModel ordermodel = new OrderModel();

            long rsid = db.return_status.Where(x => x.return_status1 == "pending").SingleOrDefault().return_status_id; 
            List<order_return> rlist = db.order_return.Where(x=>x.return_status_id==rsid).ToList();
            List<Order> olist = new List<Order>();
            foreach(order_return r in rlist)
            {
                Order o = db.Orders.Where(x => x.order_id == r.order_id).SingleOrDefault();
                olist.Add(o);
            }
            ordermodel.OrderList = olist;
            ordermodel.StatusList = db.order_status.ToList();
            ordermodel.OrderDetailList = db.Order_detail.ToList();
            ordermodel.ProductVariantList = db.product_variant_value.ToList();
            ordermodel.paymentmodeList = db.payment_mode.ToList();
            ViewBag.StatusList = new SelectList(ordermodel.StatusList, "status_id", "status");
            return View(ordermodel);
        }

        public ActionResult returnaccepted()
        {
            OrderModel ordermodel = new OrderModel();

            long rsid = db.return_status.Where(x => x.return_status1 == "accepted").SingleOrDefault().return_status_id;
            List<order_return> rlist = db.order_return.Where(x => x.return_status_id == rsid).ToList();
            List<Order> olist = new List<Order>();
            foreach (order_return r in rlist)
            {
                Order o = db.Orders.Where(x => x.order_id == r.order_id).SingleOrDefault();
                olist.Add(o);
            }
            ordermodel.orderReturnList = rlist;
            ordermodel.OrderList = olist;
            ordermodel.StatusList = db.order_status.ToList();
            ordermodel.OrderDetailList = db.Order_detail.ToList();
            ordermodel.ProductVariantList = db.product_variant_value.ToList();
            ordermodel.paymentmodeList = db.payment_mode.ToList();
            ViewBag.StatusList = new SelectList(ordermodel.StatusList, "status_id", "status");
            return View(ordermodel);
        }

        public ActionResult returnrejected()
        {
            OrderModel ordermodel = new OrderModel();

            long rsid = db.return_status.Where(x => x.return_status1 == "rejected").SingleOrDefault().return_status_id;
            List<order_return> rlist = db.order_return.Where(x => x.return_status_id == rsid).ToList();
            List<Order> olist = new List<Order>();
            foreach (order_return r in rlist)
            {
                Order o = db.Orders.Where(x => x.order_id == r.order_id).SingleOrDefault();
                olist.Add(o);
            }
            ordermodel.OrderList = olist;
            ordermodel.StatusList = db.order_status.ToList();
            ordermodel.OrderDetailList = db.Order_detail.ToList();
            ordermodel.ProductVariantList = db.product_variant_value.ToList();
            ordermodel.paymentmodeList = db.payment_mode.ToList();
            ViewBag.StatusList = new SelectList(ordermodel.StatusList, "status_id", "status");
            return View(ordermodel);
        }


        public ActionResult rejectrequest(string id)
        {
            long oid = Convert.ToInt64(id);
            order_return or = db.order_return.Where(x=>x.order_id==oid).SingleOrDefault();
            
            long rsid = db.return_status.Where(x => x.return_status1 == "rejected").SingleOrDefault().return_status_id;
            or.return_status_id=rsid;
            db.SaveChanges();
            return RedirectToAction("returnpanding", "admin", new { area="admin"});
        }

        public  ActionResult acceptrequest(string id,OrderModel model)
        {
            long oid = Convert.ToInt64(id);
            order_return or = db.order_return.Where(x => x.order_id == oid).SingleOrDefault();

            long rsid = db.return_status.Where(x => x.return_status1 == "accepted").SingleOrDefault().return_status_id;
            or.return_status_id = rsid;
            or.return_pickup_date = model.pickupDate.Date;
            db.SaveChanges();
            return RedirectToAction("returnpanding", "admin", new { area = "admin" });
        }
        public JsonResult recentalert()
        {
            List<RecentOrder> data = new List<RecentOrder>();
            long order_status_id = db.order_status.Where(x => x.status == "waiting for confirm your order").SingleOrDefault().status_id;
            DateTime today = System.DateTime.Now.Date;
            List<Order> olist = db.Orders.Where(x => x.status_id == order_status_id && x.order_date == today).OrderByDescending(x => x.order_date).Take(5).ToList();

            foreach (Order o in olist)
            {
                RecentOrder rolist = new RecentOrder();
                List<Order_detail> odlist = db.Order_detail.Where(x => x.order_id == o.order_id).ToList();

                List<string> plist = new List<string>();
                foreach (Order_detail od in odlist)
                {
                    product_variant_value pv = db.product_variant_value.Where(x => x.product_variant_id == od.product_variant_id).SingleOrDefault();
                    plist.Add(pv.product_variant_title);

                }
                rolist.productlist = plist;
                rolist.customername = db.User_account.Where(x => x.user_id == o.user_id).SingleOrDefault().name;
                rolist.orderdate = o.order_date.Date.ToString("dd/MM/yyyy");

                data.Add(rolist);
            }
            int count = data.Count;

            return Json(new
            {
                data = data,
                count = count
            }, JsonRequestBehavior.DenyGet);
        }
    }
}