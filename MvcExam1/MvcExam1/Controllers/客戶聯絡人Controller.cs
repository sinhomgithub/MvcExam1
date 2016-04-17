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
    public class 客戶聯絡人Controller : BaseController
    {

        // GET: 客戶聯絡人                
        public ActionResult Index(string keyword, string titleName, string export, string sortColumn, string sortDesc, int? pageIndex)
        {
            bool isDesc = false;
            if (!String.IsNullOrEmpty(sortDesc) && sortDesc.ToLower() == "true")
                isDesc = true;

            // 設定預設排序
            if (String.IsNullOrEmpty(sortColumn))
                sortColumn = "姓名";

            var data = repo客戶聯絡人.Query(keyword, titleName, sortColumn, isDesc);                        

            if (String.IsNullOrEmpty(export) == false)
            {
                byte[] bs = repo客戶聯絡人.Export(data);
                return this.File(bs, "application/vnd.ms-excel", "客戶聯絡人.xls");
            }

            // 執行分頁處理
            var pageNumber = pageIndex ?? 1;
            var onePageOfData = data.ToPagedList(pageNumber, 3);

            // Bind 職稱清單 (TitleName)
            IList<string> titleList = repo客戶聯絡人.DistanctTitleName();            
            ViewBag.TitleName = new SelectList(titleList, titleName);

            return View(onePageOfData);
        }

        // GET: 客戶聯絡人/Details/5        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶聯絡人 客戶聯絡人 = repo客戶聯絡人.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }
                
        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            //ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repo客戶聯絡人.Add(客戶聯絡人);
                repo客戶聯絡人.UnitOfWork.Commit();
                
                return RedirectToAction("Index");
            }

            //ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶聯絡人 客戶聯絡人 = repo客戶聯絡人.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }

            //ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);

            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]        
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db = repo客戶聯絡人.UnitOfWork.Context;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                repo客戶聯絡人.UnitOfWork.Commit();
                
                return RedirectToAction("Index");
            }
            //ViewBag.客戶Id = new SelectList(repo客戶資料.All(), "Id", "客戶名稱");

            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶聯絡人 客戶聯絡人 = repo客戶聯絡人.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }            

            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]        
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = repo客戶聯絡人.Find(id);

            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }

            // 將刪除 Mark 為 true, 不做真的刪除
            repo客戶聯絡人.Delete(客戶聯絡人);

            try
            {
                repo客戶聯絡人.UnitOfWork.Commit();
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
