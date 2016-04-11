using MvcExam1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcExam1.Controllers
{

    [Authorize]
    public class AccountController : BaseController
    {


        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel data)
        {
            if(repo客戶資料.CheckLogin(data.帳號, data.密碼))
            {
                //FormsAuthentication.RedirectFromLoginPage(data.帳號, false);   // 建立登入憑證                
                //return RedirectToAction("Index", "Home");   // 登入後轉到 /Home/Index

                string role = "Clients";
                if (data.帳號 == "admin")
                {
                    role = "Administrators";
                }

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        data.帳號,
                        DateTime.Now,
                        DateTime.Now.AddYears(50),
                        false,
                        role,
                        "/");

                string encrypetedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypetedTicket);                
                authCookie.Expires = ticket.Expiration;                
                Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");   // 登入後轉到 /Home/Index
            }

            ViewBag.ErrorMessage = "登入失敗, 請檢查帳號密碼 !";

            return View();
        }


        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Index", "Home");            
        }

        public ActionResult EditProfile()
        {
            string accountStr = User.Identity.Name;
            var data = repo客戶資料.Find(accountStr);
            if(data == null)
            {
                return HttpNotFound();
            }

            Response.Write("Administrators Role=" + User.IsInRole("Administrators"));

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(int id, string 密碼)
        {
            var data = repo客戶資料.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            // 手動 Model Bind
            if (this.TryUpdateModel(data, new string[] { "電話","傳真","地址","Email" }))
            {
                data.密碼 = repo客戶資料.EncryptPassowrd(密碼);     // 把密碼加密

                repo客戶資料.UnitOfWork.Commit();
                return RedirectToAction("Index", "Home");
            }

            return View(data);
        }





    }
}