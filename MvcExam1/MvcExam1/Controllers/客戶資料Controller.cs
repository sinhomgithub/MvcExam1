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

namespace MvcExam1.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶資料
        public ActionResult Index(string keyword, string 客戶分類, string export)
        {
            // 搜尋 keyword 與客戶分類            
            var data = repo客戶資料.Query(keyword, 客戶分類);
                       
            ViewBag.客戶分類 = new SelectList(repo客戶分類清單.All(), "客戶分類", "客戶分類");   // 取出客戶分類清單的資料

            // 匯出
            if (String.IsNullOrEmpty(export) == false)
            {
                byte[] bs = repo客戶資料.Export(data);
                return this.File(bs, "application/vnd.ms-excel", "客戶資料.xls");
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
        public ActionResult Details(IList<MvcExam1.ViewModels.客戶資料BatchUpdateViewModel> data, int customId)
        {
            var 客戶資料data = repo客戶資料.Find(customId);
            var 客戶聯絡人data = repo客戶聯絡人.QueryBy客戶Id(customId);

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

            // 比對失敗, 把更新的內容, 存到取出的資料 
            // (因為 EditorFor 的 name 對不上 Model, 只能用這種方式, 自己處理 TextBox 的修改)
            foreach(var row in 客戶聯絡人data)
            {
                var item = data.Where(p => p.Id == row.Id).FirstOrDefault();
                if (item == null)
                    continue;

                row.職稱 = item.職稱;
                row.手機 = item.手機;
                row.電話 = item.電話;     
            }

            // 更新失敗重新顯示內容
            ViewData.Model = 客戶資料data;
            ViewBag.客戶聯絡人 = 客戶聯絡人data;

            return View("Details");
        }



        // GET: 客戶資料/Create
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
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo客戶資料.Add(客戶資料);
                repo客戶資料.UnitOfWork.Commit();
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
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,是否已刪除")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = (客戶資料Entities)repo客戶資料.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                repo客戶資料.UnitOfWork.Commit();
                
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



    }
}
