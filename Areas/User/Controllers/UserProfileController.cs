using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce.Areas.User.Controllers
{
    public class UserProfileController : Controller
    {

        Entities1 db = new Entities1();

        [HttpGet]
        public ActionResult profile()
        {

            if (Session["user_id"] == null)
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                UserDetails user = new UserDetails();
                int id = Convert.ToInt32(Session["user_id"]);
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
        public ActionResult profile(UserDetails model)
        {

            int flag = 0;
            int id = Convert.ToInt32(Session["user_id"]);
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

            UserDetails userd = new UserDetails();
            userd.user_account = db.User_account.Where(x => x.user_id == id).SingleOrDefault();
            userd.addresslist = db.User_address.Where(x => x.user_id == id).ToList();
            userd.cityList = db.cities.ToList();
            ViewBag.cityList = new SelectList(userd.cityList, "city_id", "city_name");
            userd.countryList = db.Countries.ToList();
            ViewBag.countryList = new SelectList(userd.countryList, "country_id", "country_name");
            userd.addtypeList = db.address_type.ToList();
            ViewBag.addtypeList = new SelectList(userd.addtypeList, "address_type_id", "address_type1");
            return View(userd);

        }


        [HttpGet]
        public ActionResult addressbook()
        {

            if (Session["user_id"] == null)
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                UserDetails user = new UserDetails();
                int id = Convert.ToInt32(Session["user_id"]);
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
        public ActionResult addressbook(UserDetails model)
        {
            User_address add = new User_address();
            add.address = model.address;
            add.pincode = model.pincode;
            add.city_id = model.city_id;
            add.country_id = model.city_id;
            int id = Convert.ToInt32(Session["user_id"]);
            add.user_id = id;
            add.address_type_id = model.address_type_id;
            add.isdefault = 0;
            db.User_address.Add(add);
            db.SaveChanges();

            return RedirectToAction("addressbook", "UserProfile", new { area = "User" });
        }

        

        public ActionResult removeAddress(int id)
        {
            User_address add = db.User_address.Where(x => x.user_add_id == id).SingleOrDefault();
            db.User_address.Remove(add);
            db.SaveChanges();
            return RedirectToAction("addressbook", "UserProfile", new { area = "User" });
        }

        [HttpGet]
        public ActionResult editAddress(int id)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("loginAdmin", "Login", new { area = "" });
            }
            else
            {
                UserDetails eadd = new UserDetails();
                User_address uadd = db.User_address.Where(x => x.user_add_id == id).SingleOrDefault();
                eadd.address = uadd.address;
                eadd.city_id = uadd.city_id;
                eadd.country_id = uadd.country_id;
                eadd.pincode = uadd.pincode;
                eadd.address_type_id = uadd.address_type_id;

                eadd.cityList = db.cities.ToList();
                ViewBag.cityList = new SelectList(eadd.cityList, "city_id", "city_name");
                eadd.countryList = db.Countries.ToList();
                ViewBag.countryList = new SelectList(eadd.countryList, "country_id", "country_name");
                eadd.addtypeList = db.address_type.ToList();
                ViewBag.addtypeList = new SelectList(eadd.addtypeList, "address_type_id", "address_type1");
                return View(eadd);
            }
        }
        

        [HttpPost]
        public ActionResult editAddress(int id, UserDetails model)
        {
            User_address nadd = db.User_address.Where(x => x.user_add_id == id).SingleOrDefault();
            nadd.address = model.address;
            nadd.pincode = model.pincode;
            nadd.city_id = model.city_id;
            nadd.country_id = model.country_id;
            nadd.address_type_id = model.address_type_id;
            db.SaveChanges();
            return RedirectToAction("addressbook", "UserProfile", new { area = "User" });
        }
    }
}