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

            // 將 data 值填入輸入用的 ViewModel
            EditProfileViewModel model = new EditProfileViewModel();
            model.Email = data.Email;
            model.傳真 = data.傳真;
            model.地址 = data.地址;            
            model.客戶分類 = data.客戶分類;
            model.客戶名稱 = data.客戶名稱;
            model.帳號 = data.帳號;
            model.統一編號 = data.統一編號;
            model.電話 = data.電話;

            ViewData.Model = model;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(FormCollection form)
        {
            string accountStr = User.Identity.Name;     // 從登入身份取出個人資訊
            var data = repo客戶資料.Find(accountStr);

            if (data == null)
            {
                return HttpNotFound();
            }

            EditProfileViewModel model = new EditProfileViewModel();
            
            // 手動 Model Bind,  讓 model 填入 form 的內容
            if (this.TryUpdateModel(model, new string[] { "密碼", "確認密碼", "電話", "傳真", "地址", "Email" }))
            {
                // 設定要修改的欄位值
                data.密碼 = repo客戶資料.EncryptPassowrd(model.密碼);     // 把密碼加密                
                data.Email = model.Email;
                data.傳真 = model.傳真;
                data.地址 = model.地址;                
                data.電話 = model.電話;

                repo客戶資料.UnitOfWork.Commit();
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }





    }
}