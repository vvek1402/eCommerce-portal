using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce.Controllers
{
    public class LoginController : Controller
    {
        Entities1 db = new Entities1();
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
       
        
        
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(User_account User)
        {
            string md5password=GetMd5Hash(User.password);
            User_account obj = db.User_account.Where(x => x.email_id == User.email_id && x.password == md5password && x.role_id == 2).SingleOrDefault();
            if (obj != null)
            {
                    Session["user_id"] = obj.user_id.ToString();
                    Session["user_name"] = obj.name.ToString();
                return RedirectToAction("home", "Home", new { area = "User" });
            }
            else
            {
                ViewBag.error = "*Invalid Username or password";
            }
            return View();
        }

        [HttpGet]
        public ActionResult loginAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult loginAdmin(User_account User)
        {
            string md5password = GetMd5Hash(User.password);
            User_account obj = db.User_account.Where(x => x.email_id == User.email_id && x.password == md5password && x.role_id == 1).SingleOrDefault();
            if (obj != null)
            {
                    Session["user_id"] = obj.user_id.ToString();
                    Session["user_name"] = obj.name.ToString();
                return RedirectToAction("dashboard", "Admin", new { area = "admin" });
            }
            else
            {
                ViewBag.error = "*Invalid Username or password";
            }
            return View();
        }


        [HttpGet]
        public ActionResult registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registration(User_account User)
        {
            User_account newuser = new User_account();
            newuser.name = User.name;
            newuser.email_id = User.email_id;
            newuser.password = GetMd5Hash(User.password);
            newuser.mobile_no = User.mobile_no;
            newuser.role_id = 2;
            User_account obj = db.User_account.Where(x => x.email_id == User.email_id ).SingleOrDefault();
            if(obj != null)
            {
                ViewBag.error = "Email id already have account";
            }
            else
            {
                User_account obj1 = db.User_account.Where(x => x.mobile_no == User.mobile_no).SingleOrDefault();
                if(obj1!=null)
                {
                    ViewBag.error = "mobile no. already have account";
                }
                else
                {
                    db.User_account.Add(newuser);
                    db.SaveChanges();
                    return RedirectToAction("login","Login");
                }
            }
            
            return View();
        }
        public ActionResult logout()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("login", "Login");
        }

        
    }
}