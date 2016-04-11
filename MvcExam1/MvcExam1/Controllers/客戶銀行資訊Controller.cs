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
using System.Data.Entity.Infrastructure;
using PagedList;

namespace MvcExam1.Controllers
{

    [Authorize(Roles = "Administrators")]
    public class 客戶銀行資訊Controller : BaseController
    {


        // GET: 客戶銀行資訊                
        public ActionResult Index(string keyword, string export, int? pageIndex)
        {
            var data = repo客戶銀行資訊.Query(keyword);

            if (String.IsNullOrEmpty(export) == false)
            {
                byte[] bs = repo客戶銀行資訊.Export(data);
                return this.File(bs, "application/vnd.ms-excel", "客戶銀行資訊.xls");
            }


            // 執行分頁處理
            var pageNumber = pageIndex ?? 1;
            var onePageOfData = data.ToPagedList(pageNumber, 2);

            return View(onePageOfData);
        }


        // GET: 客戶銀行資訊/Details/5                
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            客戶銀行資訊 客戶銀行資訊 = repo客戶銀行資訊.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }

            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create                
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]        
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否已刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repo客戶銀行資訊.Add(客戶銀行資訊);
                repo客戶銀行資訊.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5                
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repo客戶銀行資訊.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]        
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否已刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db = (客戶資料Entities)repo客戶銀行資訊.UnitOfWork.Context;
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                repo客戶銀行資訊.UnitOfWork.Commit();                
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5                
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶銀行資訊 客戶銀行資訊 = repo客戶銀行資訊.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]        
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = repo客戶銀行資訊.Find(id);

            if (客戶銀行資訊 == null)
            {
                return HttpNotFound("客戶銀行資訊, id=" + id);
            }

            // 將刪除 Mark 為 true, 不做真的刪除
            repo客戶銀行資訊.Delete(客戶銀行資訊);

            try
            {
                repo客戶銀行資訊.UnitOfWork.Commit();                
            }
            catch (Exception ex)
            {
                string errorStr = AppHelper.GetExceptionString(ex);
                ViewBag.ErrorMessage = errorStr;
            }

            return RedirectToAction("Index");
        }

       
    }
}
