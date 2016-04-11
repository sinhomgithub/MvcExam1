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
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data.Entity.Infrastructure;
using PagedList.Mvc;
using PagedList;

namespace MvcExam1.Controllers
{

    [Authorize(Roles ="Administrators")]
    public class 客戶資料Controller : BaseController
    {

        
        [HandleError(ExceptionType = typeof(Exception), View = "GeneralError")]
        // GET: 客戶資料        
        public ActionResult Index(string keyword, string 客戶分類, string export, string sortColumn, string sortDesc, int? pageIndex)
        {            
            // 搜尋 keyword 與客戶分類, 並處理排序
            bool isDesc = false;
            if (!String.IsNullOrEmpty(sortDesc) && sortDesc.ToLower() == "true")
                isDesc = true;

            if (String.IsNullOrEmpty(sortColumn))
                sortColumn = "客戶名稱";

            var data = repo客戶資料.Query(keyword, 客戶分類, sortColumn, isDesc);
                                  

            ViewBag.客戶分類 = new SelectList(repo客戶分類清單.All(), "客戶分類", "客戶分類");   // 取出客戶分類清單的資料

            // 匯出
            if (String.IsNullOrEmpty(export) == false)
            {
                byte[] bs = repo客戶資料.Export(data);
                return this.File(bs, "application/vnd.ms-excel", "客戶資料.xls");
            }

            ViewBag.sortColumn = sortColumn;
            ViewBag.sortDesc = isDesc.ToString();
            
            // 執行分頁處理
            var pageNumber = pageIndex ?? 1;
            var onePageOfData = data.ToPagedList(pageNumber, 3);
            
            return View(onePageOfData);
        }

        // GET: 客戶資料/Details/5
        [HandleError(ExceptionType = typeof(Exception), View = "GeneralError")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var 客戶資料data = repo客戶資料.Find(id.Value);
            if (客戶資料data == null)
            {
                return HttpNotFound();
            }

            var 客戶聯絡人data = repo客戶聯絡人.QueryBy客戶Id(id.Value);
            
            ViewData.Model = 客戶資料data;
            ViewBag.客戶聯絡人 = 客戶聯絡人data;            
                                    
            return View();
        }

        /// <summary>
        /// 批次修改連絡人清單
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]
        public ActionResult Details(IList<MvcExam1.ViewModels.客戶資料DetailsBatchUpdateViewModel> data, int 客戶資料Id)
        {        
            var 客戶資料data = repo客戶資料.Find(客戶資料Id);
            var 客戶聯絡人data = repo客戶聯絡人.QueryBy客戶Id(客戶資料Id);

            // 如果比對合法
            if (data != null && ModelState.IsValid)
            {
                foreach (var item in data)
                {
                    var row = repo客戶聯絡人.Find(item.Id);
                    row.職稱 = item.職稱;
                    row.手機 = item.手機;
                    row.電話 = item.電話;
                }
                repo客戶聯絡人.UnitOfWork.Commit();

                return RedirectToAction("Index");   // 更新完成, 返回 Index
            }

            // 更新失敗重新顯示內容            
            ViewData.Model = 客戶資料data;
            ViewBag.客戶聯絡人 = 客戶聯絡人data;            

            return View();
        }



        // GET: 客戶資料/Create        
        [HandleError(ExceptionType = typeof(Exception), View = "GeneralError")]
        public ActionResult Create()
        {           
            ViewBag.客戶分類 = new SelectList(repo客戶分類清單.All(), "客戶分類", "客戶分類");   // 取出客戶分類清單的資料

            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]        
        public ActionResult Create([Bind(Include = "Id,帳號, 密碼, 客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                客戶資料.密碼 = repo客戶資料.EncryptPassowrd(客戶資料.密碼);     // 把密碼加密
                repo客戶資料.Add(客戶資料);
                repo客戶資料.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }            

            // 驗證不過
            ViewBag.客戶分類 = new SelectList(repo客戶分類清單.All(), "客戶分類", "客戶分類", 客戶資料.客戶分類);   // 取出客戶分類清單的資料   

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5                       
        [HandleError(ExceptionType = typeof(Exception), View = "GeneralError")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);
            
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            ViewBag.客戶分類 = new SelectList(repo客戶分類清單.All(), "客戶分類", "客戶分類", 客戶資料.客戶分類);   // 取出客戶分類清單的資料          

            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]        
        public ActionResult Edit(int id, string 密碼)
        {

            var data = repo客戶資料.Find(id);
            if (data == null)
                return HttpNotFound();

            // 手動 Model Bind
            if (this.TryUpdateModel(data, new string[] { "帳號", "客戶名稱", "統一編號", "電話", "傳真", "地址", "Email", "客戶分類" }))
            {                
                data.密碼 = repo客戶資料.EncryptPassowrd(密碼);     // 把密碼加密
                repo客戶資料.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            

            // 驗證不過
            ViewBag.客戶分類 = new SelectList(repo客戶分類清單.All(), "客戶分類", "客戶分類", data.客戶分類);   // 取出客戶分類清單的資料          

            return View(data);
        }

        // GET: 客戶資料/Delete/5        
        [HandleError(ExceptionType = typeof(Exception), View = "GeneralError")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo客戶資料.Find(id.Value);

            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseError")]        
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo客戶資料.Find(id);

            if (客戶資料 == null)
            {
                return HttpNotFound("客戶資料找不到, id=" + id);
            }

            // 將刪除 Mark 為 true, 不做真的刪除
            repo客戶資料.Delete(客戶資料);

            try
            {
                repo客戶資料.UnitOfWork.Commit();                
            }
            catch (Exception ex)
            {
                string errorStr = AppHelper.GetExceptionString(ex);
                ViewBag.ErrorMessage = errorStr;
            }

            return RedirectToAction("Index");
        }

        [HandleError(ExceptionType = typeof(Exception), View = "Error")]
        public ActionResult MakeException()
        {
            throw new Exception("一般錯誤 !");
        }
        
        [HandleError(ExceptionType = typeof(DbUpdateException), View = "DatabaseException")]
        public ActionResult MakeDatabaseException()
        {
            throw new Exception("資料庫更新錯誤 !");
        }
        
        [HandleError(ExceptionType =typeof(ArgumentException), View ="ArgumentError")]
        public ActionResult MakeArgumentException()
        {
            throw new ArgumentException("參數錯誤 !");            
        }

        [HandleError(ExceptionType = typeof(IOException), View = "IOError")]
        public ActionResult MakeIOException()
        {
            throw new IOException("I/O 處理錯誤 !");            
        }


    }
}
