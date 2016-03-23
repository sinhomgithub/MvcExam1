using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcExam1.Models;
using MvcExam1.Library;

namespace MvcExam1.Controllers
{
    public class 客戶資料Controller : Controller
    {
        private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶資料
        public ActionResult Index(string keyword)
        {
            var data = db.客戶資料.AsQueryable();
            data = data.Where(p => p.是否已刪除 == false);   // 刪除的資料不能出現

            if (String.IsNullOrWhiteSpace(Request["keyword"]) == false)
            {
                string keywordStr = Request["keyword"].Trim();
                data = data.Where(p => 
                    p.客戶名稱.IndexOf(keywordStr) != -1 ||
                    p.統一編號.IndexOf(keywordStr) != -1 ||
                    p.電話.IndexOf(keywordStr) != -1 ||
                    p.傳真.IndexOf(keywordStr) != -1 ||
                    p.地址.IndexOf(keywordStr) != -1 ||
                    p.Email.IndexOf(keywordStr) != -1
                );                
            }
            
            return View(data);
        }             

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null || 客戶資料.是否已刪除 == true)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.客戶資料.Add(客戶資料);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            
            if (客戶資料 == null || 客戶資料.是否已刪除 == true)
            {
                return HttpNotFound();
            }                                   

            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null || 客戶資料.是否已刪除 == true)
            {
                return HttpNotFound();
            }

            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = db.客戶資料.Find(id);

            if (客戶資料 == null || 客戶資料.是否已刪除 == true)
            {
                return HttpNotFound("客戶資料找不到, id=" + id);
            }

            // 將刪除 Mark 為 true, 不做真的刪除
            客戶資料.是否已刪除 = true;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string errorStr = AppHelper.GetExceptionString(ex);
                ViewBag.ErrorMessage = errorStr;
            }


            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
